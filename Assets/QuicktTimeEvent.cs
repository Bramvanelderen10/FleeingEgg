using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickTimeEvent
{
    public enum Type
    {
        None,
        A,
        B,
        X,
        Y,
    }

    public enum Difficulty
    {        
        Easy,
        Normal,
        Hard,
        VeryHard,
    }

    public class Utils
    {

        

        public static string ConvertTypeToString(Type type)
        {
            string typeString = "";
            switch(type)
            {
                case Type.None:
                    typeString = "None";
                    break;
                case Type.A:
                    typeString = "A";
                    break;
                case Type.B:
                    typeString = "B";
                    break;
                case Type.X:
                    typeString = "X";
                    break;
                case Type.Y:
                    typeString = "Y";
                    break;
            }

            return typeString;
        }
    }

    public class ComboOld
    {
        public static float[,] GetCombo(Difficulty difficulty, int index)
        {
            List<float[,]> combos = new List<float[,]>();
            combos.Add(new float[1, 2] { { 0f, 0f} }); //Basic QTE
            combos.Add(new float[3, 2] { { 0f, 0f }, { -1.4f, -1.4f }, { 1.4f, 1.4f } }); //3 QTE's in a long diagonal row
            combos.Add(new float[3, 2] { { 0f, 0f }, { 1.4f, -1.4f }, { -1.4f, 1.4f } }); //3 QTE's in a long reversed diagonal row
            combos.Add(new float[3, 2] { { 0f, 0f }, { -0.7f, -0.7f }, { 0.7f, 0.7f } }); //3 QTE's in a short diagonal row
            combos.Add(new float[3, 2] { { 0f, 0f }, { 0.7f, -0.7f }, { -0.7f, 0.7f } }); //3 QTE's in a short reversed diagonal row
            combos.Add(new float[5, 2] { { 0f, 0f }, { 0.76f, -1.66f }, { 1.07f, 1.99f }, { 1.76f, -0.97f }, { 2.2f, 0.1f } }); //Advanced zigzag combo

            Random rnd = new Random();

            return combos[index];
        }

        public static int GetComboCount()
        {
            return 5;
        }
    }

}
