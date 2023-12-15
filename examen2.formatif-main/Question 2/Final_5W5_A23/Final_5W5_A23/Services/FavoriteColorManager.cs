namespace FavoriteColor.Services
{
    public enum ColorChoice
    {
        NO_COLOR,
        RED,
        GREEN,
        BLUE
    }

    public class FavoriteColorManager
    {
        public int[] NbFavoritesPerColor { get; private set; } = new int[4];

        private Dictionary<string, ColorChoice> _usersFavoriteColors = new Dictionary<string, ColorChoice>();

        public void AddUserWithNoColor(string newConnectionId)
        {
            _usersFavoriteColors[newConnectionId] = ColorChoice.NO_COLOR;
            NbFavoritesPerColor[(int)ColorChoice.NO_COLOR]++;
        }

        public ColorChoice RemoveUser(string connectionId)
        {
            // On retire l'utilisateur
            ColorChoice color = _usersFavoriteColors[connectionId];
            _usersFavoriteColors.Remove(connectionId);
            
            // On met à jour le nombre d'utilisateur qui aime cette couleur
            NbFavoritesPerColor[(int)color]--;
            
            return color;
        }

        public ColorChoice ChangeFavoriteColor(string connectionId, ColorChoice newColor)
        {
            // On change le choix de couleur pour cet utilisateur
            ColorChoice oldColor = _usersFavoriteColors[connectionId];
            _usersFavoriteColors[connectionId] = newColor;
            
            // On met à jour le nombre d'utilisateurs qui aiment chaque couleur
            NbFavoritesPerColor[(int)oldColor]--;
            NbFavoritesPerColor[(int)newColor]++;

            return oldColor;
        }
        
        public int GetNbFavorites(ColorChoice color)
        {
            return NbFavoritesPerColor[(int)color];
        }

        public string GetGroupName(string connectionId)
        {
            return GetGroupName(GetFavoriteColor(connectionId));
        }

        public ColorChoice GetFavoriteColor(string connectionId)
        {
            return _usersFavoriteColors[connectionId];
        }

        public string GetGroupName(ColorChoice color)
        {
            return color.ToString() + "Group";
        }
    }
}
