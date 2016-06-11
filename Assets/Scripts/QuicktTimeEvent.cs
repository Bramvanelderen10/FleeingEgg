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
        EASY,
        NORMAL,
        HARD,
        INSANE
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

        public static Type GetRandomType(System.Random rnd, List<Type> excludeList = null, bool first = true)
        {
            if (excludeList == null)
                excludeList = new List<Type>();

            if (first)
                excludeList.Add(Type.None);
            
            Type selected = (Type)rnd.Next(0, Enum.GetNames(typeof(Type)).Length);
            if (excludeList.Contains(selected))
            {
                selected = GetRandomType(rnd, excludeList, false);
            }

            excludeList.Remove(Type.None);
            return selected;
        }
    }
}
