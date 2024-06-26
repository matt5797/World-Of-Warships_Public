using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using WOW.Data;

namespace WOW
{
    public class BallisiticManager : MonoBehaviour
    {
        struct FiringTable
        {
            public int ID;
            public int Shell;
            public float Angle;
            public float X;
            public float Y;
        }

        public static string m_DatabaseFileName = "WOWS.db";
        public static string m_TableName = "FiringTable";
        private static DatabaseAccess m_DatabaseAccess;

        public Dictionary<int, Dictionary<int, float>> m_FiringTable = new Dictionary<int, Dictionary<int, float>>();

        public static BallisiticManager Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            string filePath = Path.Combine(Application.streamingAssetsPath, m_DatabaseFileName);
            Debug.Log(filePath);
            m_DatabaseAccess = new DatabaseAccess("data source = " + filePath);
        }
        
        void Start()
        {
            //print(m_DatabaseFileName);
            /*string filePath = Path.Combine(Application.streamingAssetsPath, m_DatabaseFileName);
            Debug.Log(filePath);
            m_DatabaseAccess = new DatabaseAccess("data source = " + filePath);*/

            //print(GetAngle(2, 12000));
        }

        void InsertFiringTable(int shell)
        {
            //string sql = string.Format("SELECT X, Angle FROM FiringTable WHERE Shell=={0} AND (Angle*100)%100==0", shell);
            string sql = string.Format("SELECT X, Angle FROM FiringTable WHERE Shell=={0}", shell);
            var res = m_DatabaseAccess.ExecuteQuery(sql);
            Dictionary<int, float> insertData = new Dictionary<int, float>();
            while (res.Read())
            {
                insertData.TryAdd((int)res.GetFloat(0), res.GetFloat(1));
            }
            m_FiringTable.Add(shell, insertData);
        }

        public float GetAngle(int shell, int targetX)
        {
            if (!m_FiringTable.ContainsKey(shell))
            {
                InsertFiringTable(shell);
            }

            float nearestAngle = 0;
            int nearest = int.MaxValue;
            foreach (var item in m_FiringTable[shell])
            {
                if (Mathf.Abs(item.Key - targetX) < nearest)
                {
                    nearestAngle = item.Value;
                    nearest = Mathf.Abs(item.Key - targetX);
                }
            }

            return nearestAngle;
        }

        public int GetShellID(string ShellName)
        {
            string sql = string.Format("SELECT ID from Shells where name==\"{0}\";", ShellName);
            var res = m_DatabaseAccess.ExecuteQuery(sql);
            if (res.Read())
            {
                return res.GetInt32(0);
            }
            return 1;
        }

        public ShellData GetShellData(int shellID)
        {
            ShellData shellData = new ShellData();
            string sql = string.Format("SELECT * from Shells where ID=={0};", shellID);
            var res = m_DatabaseAccess.ExecuteQuery(sql);
            if (res.Read())
            {
                Debug.Log(res.GetString(1));
                shellData.alphaPiercingCS = res.GetFloat(2);
                shellData.alphaPiercingHE = res.GetFloat(3);
                shellData.bulletAirDrag = res.GetFloat(4);
                shellData.bulletAlwaysRicochetAt = res.GetFloat(5);
                shellData.bulletCapNormalizeMaxAngle = res.GetFloat(6);
                shellData.bulletDetonator = res.GetFloat(7);
                shellData.bulletDetonatorThreshold = res.GetFloat(8);
                shellData.bulletDiametr = res.GetFloat(9);
                shellData.bulletKrupp = res.GetFloat(10);
                shellData.bulletMass = res.GetFloat(11);
                shellData.bulletRicochetAt = res.GetFloat(12);
                shellData.bulletSpeed = res.GetFloat(13);
            }
            return shellData;
        }
        
        private void OnDestroy()
        {
            m_DatabaseAccess.CloseSqlConnection();
        }
    }
    
}