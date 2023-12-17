import { Component } from '@angular/core';
import * as signalR from "@microsoft/signalr"

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Couleur Favorite';

  private hubConnection?: signalR.HubConnection;
  isConnected: boolean = false;

  nbFavoritesPerColor: number[] = [0, 0, 0, 0];

  recentMessages: string[] = [];
  text = "";

  selectedIndex = 0;

  favoriteColors: any[] = [
    {name: "Aucune", backgroundColor: "black"},
    {name: "Rouge", backgroundColor: "red"},
    {name: "Vert", backgroundColor: "green"},
    {name: "Bleu", backgroundColor: "cyan"}];

  connect() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5004/hubs/favoriteColor')
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('La connexion est live!');
        // TODO: Mettre à jour la variable isConnected
        this.isConnected = true;
        // TODO: Enregistrer et gérer les évènements suivants:
        // InitFavorites (mettre nbFavoritesPerColor à jour)
        this.hubConnection!.on('InitFavorites', (data) =>{
          this.nbFavoritesPerColor = data;
        })
        // UpdateFavorites (mettre nbFavoritesPerColor à jour)
        this.hubConnection!.on('UpdateFavorites', (colorIndex, nbFavorites) =>{
          this.nbFavoritesPerColor[colorIndex] = nbFavorites;
        })
        // ReceiveMsg (mettre recentMessages à jour)
        this.hubConnection!.on('ReceiveMsg', (data) =>{
          this.recentMessages.push(data);
        })
        // Disconnect
        this.hubConnection!.on('Disconnect', (data) =>{
          this.nbFavoritesPerColor = data;
        })
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  disconnect() {
    // TODO: Se déconnecter du Hub
    // TODO: Mettre isConnected à jour
    this.hubConnection!.invoke("OnDisconnectedAsync").then(() => {
      this.hubConnection!.stop().then(() => {
        this.isConnected = false;
        this.selectedIndex = 0;
      });
    });
  }

  chooseColor(colorIndex:number) {
    this.selectedIndex = colorIndex;
    // TODO: Appeler ChooseColor sur le Hub avec la nouvelle couleur
    this.hubConnection!.invoke("ChooseColor", this.selectedIndex);
    // On efface les messages récent en changeant de couleur
    this.recentMessages = [];
  }

  sendMessage() {
    // TODO: Appeler SendMessage sur le hub avec notre message qui doit être envoyé aux utilisateurs qui ont choisir la même couleur
    this.hubConnection!.invoke("SendMessage", this.text);
    // On efface le contenu de l'input text
    this.text = "";
  }
}
