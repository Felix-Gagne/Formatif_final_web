using FavoriteColor.Services;
using Microsoft.AspNetCore.SignalR;

namespace FavoriteColor.Hubs
{
    public class FavoriteColorHub : Hub
    {
        private readonly FavoriteColorManager _favoriteColorManager;

        public FavoriteColorHub(FavoriteColorManager favoriteColorManager) {
            _favoriteColorManager = favoriteColorManager;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            // Ajouter le user comme un "NoColor"
            _favoriteColorManager.AddUserWithNoColor(Context.ConnectionId);
            // Ajouter le user au groupe "NoColor"
            string noColorGroupName = _favoriteColorManager.GetGroupName(ColorChoice.NO_COLOR);
            await Groups.AddToGroupAsync(Context.ConnectionId, noColorGroupName);
            
            int nbFavorites = _favoriteColorManager.GetNbFavorites(ColorChoice.NO_COLOR);

            // TODO: Mettre le client à jour avec la quantité de favoris pour TOUTES les couleurs sur le client qui vient de se connecter avec l'event InitFavorites
            await Clients.Caller.SendAsync("InitFavorites", _favoriteColorManager.NbFavoritesPerColor);

            // TODO: Utiliser l'event UpdateFavorites pour mettre à jour la quantité pour NO_COLOR sur les clients
            await Clients.All.SendAsync("UpdateFavorites", ColorChoice.NO_COLOR);
        }

        // TODO: Quand un utilisateur se déconnecte, il faut appeler _favoriteColorManager.RemoveUser et mettre à jour la quantité pour la couleur que l'utilisateur avait sur les clients

        public async Task ChooseColor(ColorChoice newColor)
        {
            // On change la couleur
            ColorChoice oldColor = _favoriteColorManager.ChangeFavoriteColor(Context.ConnectionId, newColor);

            int nbOldColor = _favoriteColorManager.GetNbFavorites(oldColor);
            int nbNewColor = _favoriteColorManager.GetNbFavorites(newColor);

            string oldGroupName = _favoriteColorManager.GetGroupName(oldColor);
            string newGroupName = _favoriteColorManager.GetGroupName(newColor);

            // TODO: Utiliser l'event UpdateFavorites pour mettre à jour la quantité pour oldColor sur les clients
            await Clients.All.SendAsync("UpdateFavorites", oldColor, nbOldColor);

            // TODO: Utiliser l'event UpdateFavorites pour mettre à jour la quantité pour newColor sur les clients
            await Clients.All.SendAsync("UpdateFavorites", newColor);

            // TODO: Retirer l'utilisateur de son ancien groupe
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, oldGroupName);

            // TODO: Ajouter l'utilisateur à son nouveau groupe
            await Groups.AddToGroupAsync(Context.ConnectionId, newGroupName);
        }

        public async Task SendMessage(string message)
        {
            ColorChoice color = _favoriteColorManager.GetFavoriteColor(Context.ConnectionId);
            string groupName = _favoriteColorManager.GetGroupName(color);

            // TODO: Envoyer un message seulement aux utilisateurs qui ont choisi la même couleur en utilisant l'évènement ReceiveMsg
            await Clients.Group(groupName).SendAsync("ReceiveMsg", message);
        }
    }
}
