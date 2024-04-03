using Godot;
using System;

public static class DataManager {
    //untuk sekarang, data yang disave dikhususkan untuk highscore saja
    public static void Save(int highScore) {
        FileAccess fileAccess = FileAccess.Open("user://PlayerData.save", FileAccess.ModeFlags.Write);
        fileAccess.StoreString(highScore.ToString());
        fileAccess.Close();
    }

    public static int Load() {
        if (!FileAccess.FileExists("user://PlayerData.save")) {
            Save(0);
        }
        FileAccess fileAccess = FileAccess.Open("user://PlayerData.save", FileAccess.ModeFlags.Read);
        string loadedData = fileAccess.GetAsText();
        fileAccess.Close();
        return loadedData.ToInt();
    }
}
