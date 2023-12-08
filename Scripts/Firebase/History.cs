using System;
using System.Globalization;
using Firebase.Firestore;
using UnityEngine;

namespace Firebase
{
    [FirestoreData]
    public class History
    {
        public History() {}
        [FirestoreProperty] public int Stage { get; private set; }
        [FirestoreProperty] public int TotalRound { get; private set; }
        [FirestoreProperty] public string GeneratedAt { get; private set; }


        public History(int stage, int totalRound)
        {
            (Stage, TotalRound, GeneratedAt) = (stage, totalRound, DateTime.Now.ToString(CultureInfo.CurrentCulture));
        }
    }
}