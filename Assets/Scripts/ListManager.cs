using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    // categories of lists
    public static List<string> whoList = new List<string>();
    public static List<string> whereList = new List<string>();
    public static List<string> doList = new List<string>();

    public static List<string> complimentList = new List<string>();
    public static List<string> criticismList = new List<string>();

    // player new sentence
    public static List<string> playerSentences = new List<string>();
}
