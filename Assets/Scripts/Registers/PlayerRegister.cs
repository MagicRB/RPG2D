using System;

namespace RPG2D.Registers
{
    public static class PlayerRegister
    {
        private static System.Type _playerType;
        private static BaseClasses.Player _player;

        public static Type PlayerType
        {
            get { return _playerType; }
            set { _playerType = value; }
        }

        public static BaseClasses.Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public static void InstantiatePlayer()
        {
            if (_playerType != null)
            {
                _player = (BaseClasses.Player) Activator.CreateInstance(_playerType);
                _player.Init();
            }
        }
    }
}