using ClickableTransparentOverlay;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class Client 
    {
        public const int TCP_SIZE = 8192;
        public const int TIMEOUT = 30000;
        public int Port;
        public string Host;
        public TcpClient ServerSocket;
        private readonly object tcpSendLock = new object();
        private readonly object syncLock = new object();
        private Functions functions;

        private Task ListenerTask;
        private CancellationTokenSource ListenerToken;
        private readonly ManualResetEventSlim _OnResponce = new ManualResetEventSlim(false);
        private readonly object _IsConnectedLock = new object();
        private bool _IsConnected = false;

        private bool IsConnected
        {
            get
            {
                lock (_IsConnectedLock)
                {
                    return _IsConnected;
                }
            }
            set
            {
                lock (_IsConnectedLock)
                {
                    _IsConnected = value;
                }
            }
        }

        Action OnStartConnect { get; set; }
        Action OnConnected { get; set; }
        Action OnDisconnect { get; set; }
        Action OnPing { get; set; }
        Action<Exception> OnError { get; set; }


        public Client()
        {
            functions = new Functions(this);
        }

        public void StartAsync()
        {
            ListenerToken = new CancellationTokenSource();
            ListenerTask = Task.Factory.StartNew(Listener, TaskCreationOptions.LongRunning);
        }

        public void GetUpdate(ExecuteData data)
        {
            try
            {
                functions.RunUpdater(data);
            }
            catch(Exception ex) 
            {
                Server.LogError(T.GetString("yd3DAgEE7jRRzlli", "Updater Load data error: ") + ex.Message, data.Game);
                Environment.Exit(0);
            }

            _Dicsonnect();
            Environment.Exit(0);
        }

        public bool Connect(bool RaiseException)
        {
            try
            {
                _Connect();
            }
            catch (Exception ex)
            {
                _Dicsonnect();
                if (RaiseException) throw ex;
            }

            bool b = IsConnected;
            StartAsync();

            return b;
        }

        public void ReConnect()
        {
            if (ListenerTask != null && ListenerTask.Status == TaskStatus.Running)
            {
                ListenerToken.Cancel();
                ListenerTask.Wait();
                ListenerToken = new CancellationTokenSource();
                ListenerTask = Task.Factory.StartNew(Listener, ListenerTask.CreationOptions);
            }
            else
            {
                ListenerToken = new CancellationTokenSource();
                ListenerTask = Task.Factory.StartNew(Listener, ListenerTask.CreationOptions);
            }
        }

        public void ReConnectAsync()
        {
            if (ListenerTask != null && ListenerTask.Status == TaskStatus.Running)
            {
                ListenerToken.Cancel();
                ListenerTask = ListenerTask.ContinueWith(t =>
                {
                    t.Dispose();
                    ListenerToken = new CancellationTokenSource();
                    Listener();
                }, (TaskContinuationOptions)ListenerTask.CreationOptions);
            }
            else
            {
                ListenerToken = new CancellationTokenSource();
                ListenerTask = Task.Factory.StartNew(Listener, ListenerTask.CreationOptions);
            }
        }

        #region Private

        private void _Connect()
        {
            ServerSocket = new TcpClient();
            ServerSocket.ReceiveBufferSize = TCP_SIZE;
            ServerSocket.SendBufferSize = TCP_SIZE;
            ServerSocket.ReceiveTimeout = TIMEOUT;
            ServerSocket.SendTimeout = TIMEOUT;

            if (OnStartConnect != null) OnStartConnect.BeginInvoke(null, null);

            ServerSocket.Connect(Host, Port);
            if (OnConnected != null) OnConnected.BeginInvoke(null, null);
            IsConnected = true;
        }

        private void Listener()
        {
            while (true)
            {
                try
                {
                    if (ListenerToken.IsCancellationRequested) return;

                    if (!IsConnected) _Connect();

                    while (true)
                    {
                        if (ListenerToken.IsCancellationRequested) return;
                        {
                            Unit msg;
                            try
                            {
                                msg = MessageFromBinary<Unit>();
                            }
                            catch (Exception ex)
                            {
                                Server.LogError(T.GetString("gPaDG6ZZ4YsKIIqL", "MessageFromBinary Error !!!"), T.GetString("gPaDG6ZZ4YsKIIqL", "Tundra"));
                                continue;
                            }

                            GuideMessage(msg);
                        }

                    }
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    if (OnError != null) OnError.BeginInvoke(ex, null, null);
                }
                finally
                {
                    _Dicsonnect();
                }

                Thread.Sleep(2000);
            }
        }

        private Unit _syncResult;
        public void SyncResult(Unit msg)
        {
            _syncResult = msg;
            _syncResult.IsEmpty = false;
            _OnResponce.Set();
        }

        public Unit Execute(string MethodName, object[] parameters, bool IsWaitAnswer)
        {
            lock (syncLock)
            {
                _syncResult = new Unit(MethodName, parameters);
                _syncResult.IsSync = IsWaitAnswer;
                if (IsWaitAnswer)
                {
                    _OnResponce.Reset();
                    SendMessage(_syncResult);
                    _OnResponce.Wait();
                }
                else
                {

                    SendMessage(_syncResult);
                }

                if (_syncResult.IsEmpty && IsWaitAnswer)
                {
                    throw new Exception(string.Concat(T.GetString("yd3DAgEE7jRRzlli", "Ошибка при получении результата на команду \""), MethodName, "\""));
                }

                if (_syncResult.Exception != null)
                    throw _syncResult.Exception;
                return _syncResult;
            }
        }

        public void _Dicsonnect()
        {
            if (ServerSocket != null) ServerSocket.Close();
            IsConnected = false;
            OnDisconnect?.BeginInvoke(null, null);

            _OnResponce.Set();
            GC.Collect(2, GCCollectionMode.Optimized);
        }

        public  void GuideMessage(Unit unit)
        {
            if (unit.IsAnswer)
            {
                if (unit.IsSync)
                {
                    SyncResult(unit);
                }
                else
                {
                    //TODO: реализовать асинхронные вызовы
                    //Ошибка выполнения асинхронного вызова
                }
            }
            else
            {
                ProcessMessage(unit);
            }
        }

        private void ProcessMessage(Unit msg)
        {
            string MethodName = msg.Command;
            if (MethodName == T.GetString("yd3DAgEE7jRRzlli", "OnPing"))
            {
                if (!msg.IsAnswer)
                {
                    msg.IsAnswer = true;
                    SendMessage(msg);
                }
                return;
            }

            MethodInfo method = functions.GetType().GetMethod(MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            try
            {
                if (method == null)
                {
                    throw new Exception(string.Concat(T.GetString("yd3DAgEE7jRRzlli", "Метод \""), MethodName, T.GetString("yd3DAgEE7jRRzlli", "\" недоступен")));
                }

                try
                {
                    // выполняем метод интерфейса
                    msg.ReturnValue = method.Invoke(functions, msg.prms);
                }
                catch (Exception ex)
                {
                    throw ex.InnerException ?? ex;
                }

                // возвращаем ref и out параметры
                msg.prms = method.GetParameters().Select(x => x.ParameterType.IsByRef ? msg.prms[x.Position] : null).ToArray();
            }
            catch (Exception ex)
            {
                msg.Exception = ex;
            }
            finally
            {
                if (msg.IsSync)
                {
                    // возвращаем результат выполнения запроса
                    msg.IsAnswer = true;
                    SendMessage(msg);
                }
                else
                {
                    if (msg.Exception != null)
                    {
                        msg.IsAnswer = true;
                        SendMessage(msg);
                    }
                }
            }
        }

        private T MessageFromBinary<T>()
        {
            byte[] DataLength = BitConverter.GetBytes((int)1);
            
            ServerSocket.GetStream().Read(DataLength, 0, DataLength.Length);
            int len = BitConverter.ToInt32(DataLength, 0);
            byte[] BinaryData = new byte[len];
            int counter = 0;
            while (len - 50 >= ServerSocket.Available && counter <= 50)
            {
                Thread.Sleep(20);
                counter++;
            }
            ServerSocket.GetStream().Read(BinaryData, 0, BinaryData.Length);
            var jsonData = Encoding.ASCII.GetString(BinaryData);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public void SendMessage(Unit msg)
        {
            var jsonMessage = JsonConvert.SerializeObject(msg);
            var bytes = Encoding.ASCII.GetBytes(jsonMessage);
            byte[] DataLength = BitConverter.GetBytes(bytes.Length);
            byte[] DataWithHeader = DataLength.Concat(bytes).ToArray();
            
            lock (tcpSendLock)
            {
                ServerSocket.GetStream().Write(DataWithHeader, 0, DataWithHeader.Length);
            }
        }
        #endregion
    }
}