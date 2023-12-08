using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

namespace Firebase
{
    internal sealed class FirestoreDb
    {
        private FirestoreDb() {}

        private static FirestoreDb _firestoreDb;

        private FirebaseFirestore _firestore;

        public static FirestoreDb Init()
        {
            return _firestoreDb ??= new FirestoreDb();
        }
            
        public void ConnectFirebaseFirestore()
        {
            try
            {
                _firestore = FirebaseFirestore.DefaultInstance;
            }
            catch (Exception)
            {
                Debug.LogError("connexion à firebase impossible");
            }
        }

        public async Task<IEnumerable<DocumentSnapshot>> GetHistoryDocumentsSnapshot()
        { 
            var collection = await _firestore.Collection("history").GetSnapshotAsync();

            return collection.Documents;
        }

        public async void AddDataToHistory(int stage, int totalRound)
        {
            var history = new History(stage, totalRound);
            await _firestore.Collection("history").AddAsync(history);
        }
    }
}