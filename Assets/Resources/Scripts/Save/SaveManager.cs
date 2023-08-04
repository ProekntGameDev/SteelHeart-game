namespace SaveSystem
{
    public static class SaveManager
    {
        public static PlayerInteractor PlayerInteractor { get; private set; } = new PlayerInteractor();
        public static SaveObjectInteractor SaveObjectInteractor { get; private set; } = new SaveObjectInteractor();

        public static void Save()
        {
            PlayerInteractor.Repository.Save();
            SaveObjectInteractor.Repository.Save();
        }
    }
}