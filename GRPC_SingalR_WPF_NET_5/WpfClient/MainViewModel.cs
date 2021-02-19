using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using WpfClient.Utilities;

namespace WpfClient
{
  public class MainViewModel : INotifyPropertyChanged
  {
    #region SignalR

    // Verbindung zum SignalR-Server
    private HubConnection signalRConnection;

    public ActionCommand StartSignalRTankstellenabfrageCommand { get; set; }
    public ActionCommand StopSignalRTankstellenabfrageCommand { get; set; }

    // Hilfsobjekt zum Beenden des Datenempfangs
    private IDisposable tankstellenabfrageSignalRDisposable;

    private IEnumerable<CommonTypes.Tankstelle> tankstellenSignalR;

    // Tankstellendaten zur Darstellung im DataGrid
    public IEnumerable<CommonTypes.Tankstelle> TankstellenSignalR
    {
      get { return tankstellenSignalR; }
      set { tankstellenSignalR = value; OnPropertyChanged(); }
    }

    #endregion


    #region gRPC
    public ActionCommand StartGrpcTankstellenabfrageCommand { get; }
    public ActionCommand StopGrpcTankstellenabfrageCommand { get; }

    
    private IEnumerable<Services.Protos.Tankstelle> tankstellenGrpc;

    // Tankstellendaten zur Darstellung im DataGrid
    public IEnumerable<Services.Protos.Tankstelle> TankstellenGrpc
    {
      get { return tankstellenGrpc; }
      set { tankstellenGrpc = value; OnPropertyChanged(); }
    }

    // Hilfsobjekt zum Abbrechen der Datenübertragung
    private CancellationTokenSource tankstellenabfrageGrpcCancellationTokenSource;

    // Verbindung zum gRPC-Server
    private GrpcChannel channel;

    #endregion


    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    public MainViewModel()
    {
      // SignalR
      StartSignalRTankstellenabfrageCommand = new ActionCommand(StartSignalRTankstellenabfrage);
      StopSignalRTankstellenabfrageCommand = new ActionCommand(StopSignalRTankstellenabfrage) { IsEnabled = false };

      // SignalR-Verbindung vorbereiten
      signalRConnection = new HubConnectionBuilder().WithUrl("https://localhost:5001/tankstellen").Build();

      // gRPC
      StartGrpcTankstellenabfrageCommand = new ActionCommand(StartGrpcTankstellenabfrage);
      StopGrpcTankstellenabfrageCommand = new ActionCommand(StopGrpcTankstellenabfrage) { IsEnabled = false };

      // gRPC-Verbindung vorbereiten
      channel = GrpcChannel.ForAddress("https://localhost:5001");

    }

    private async void StartSignalRTankstellenabfrage()
    {
      try
      {
        // Bei Bedarf SignalR-Verbindung startet
        if (signalRConnection.State != HubConnectionState.Connected)
          await signalRConnection.StartAsync();

        // Subscription für Kraftstoffpreise einrichten
        // der Rückgabewert kann zum Beenden der Subscription verwendet werden
        tankstellenabfrageSignalRDisposable = signalRConnection.On<IEnumerable<CommonTypes.Tankstelle>>("kraftstoffpreise", liste =>
        {
          // Property und damit UI aktualisieren
          TankstellenSignalR = liste;
        });

        // Verfügbarkeit der Buttons steuern
        StartSignalRTankstellenabfrageCommand.IsEnabled = false;
        StopSignalRTankstellenabfrageCommand.IsEnabled = true;

      }
      catch (Exception ex)
      {
        Debug.WriteLine("Fehler: " + ex.Message);
      }
    }

    private void StopSignalRTankstellenabfrage()
    {
      // Subscription der Kraftstoffpreise beenden
      tankstellenabfrageSignalRDisposable?.Dispose();

      // Verfügbarkeit der Buttons steuern
      StartSignalRTankstellenabfrageCommand.IsEnabled = true;
      StopSignalRTankstellenabfrageCommand.IsEnabled = false;
    }


    private async void StartGrpcTankstellenabfrage()
    {
      // Verfügbarkeit der Buttons steuern
      StartGrpcTankstellenabfrageCommand.IsEnabled = false;
      StopGrpcTankstellenabfrageCommand.IsEnabled = true;

      // Token für möglichen Abbruch vorbereiten
      tankstellenabfrageGrpcCancellationTokenSource = new CancellationTokenSource();

      // gRPC-Client instanzieren
      var client = new Services.Protos.TankstellenGrpc.TankstellenGrpcClient(channel);

      // Aufruf einrichten, Cancellation-Token für möglichen Abbruch übergeben
      var asyncStreamCall = client.Tankstellenpreise(new() { Anzahl = 10 }, cancellationToken: tankstellenabfrageGrpcCancellationTokenSource.Token);

      // kontinuierlich aus dem Stream Daten entgegennehmen
      var stream = asyncStreamCall.ResponseStream;
      try
      {
        await foreach (var msg in stream.ReadAllAsync())
        {
          // Daten in Property übertragen -> Update des UIs
          TankstellenGrpc = msg.Liste;
        }


      }
      catch (Exception ex)
      {
        Debug.WriteLine("Fehler: " + ex.Message);
      }

      finally
      {
        // Fertig -> aufräumen
        tankstellenabfrageGrpcCancellationTokenSource.Dispose();
      }

      // Hier geht es erst weiter, wenn die Datenübertragung erfolgreich oder durch Abbruch beendet wurde

      // Verfügbarkeit der Buttons steuern
      StartGrpcTankstellenabfrageCommand.IsEnabled = true;
      StopGrpcTankstellenabfrageCommand.IsEnabled = false;

    }

    private void StopGrpcTankstellenabfrage()
    {
      // Ende der Stream-Übertragung anfordern
      tankstellenabfrageGrpcCancellationTokenSource?.Cancel();
    }


  }
}
