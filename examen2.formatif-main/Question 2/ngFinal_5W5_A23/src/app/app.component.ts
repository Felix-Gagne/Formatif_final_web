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

        // TODO: Enregistrer et gérer les évènements suivants:
        // InitFavorites (mettre nbFavoritesPerColor à jour)
        // UpdateFavorites (mettre nbFavoritesPerColor à jour)
        // ReceiveMsg (mettre recentMessages à jour)
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  disconnect() {
    // TODO: Se déconnecter du Hub
    // TODO: Mettre isConnected à jour
  }

  chooseColor(colorIndex:number) {
    this.selectedIndex = colorIndex;
    // TODO: Appeler ChooseColor sur le Hub avec la nouvelle couleur

    // On efface les messages récent en changeant de couleur
    this.recentMessages = [];
  }

  sendMessage() {
    // TODO: Appeler SendMessage sur le hub avec notre message qui doit être envoyé aux utilisateurs qui ont choisir la même couleur

    // On efface le contenu de l'input text
    this.text = "";
  }
}
