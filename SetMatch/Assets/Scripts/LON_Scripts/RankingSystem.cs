using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NS_Rank
{
    public class Player
    {
        public RankNames rankNames;
        public bool isOnline;
        public string pseudo;
        public string mail;
        public float ELO;
        public int winChain;
        public int loseChain;

        public int currentListIndex;


        public enum RankNames { Amateur, Pro, FifthLeague, FourthLeague, ThirdLeague, SecondLeague, FirstLeague, Master, Legend }
    }

    public class RankingSystem : MonoBehaviour
    {      
        //List of all players
        public List<Player> listPlayers = new List<Player>();

        //Lists by rank
        public List<Player> rankAmateur = new List<Player>();
        public List<Player> rankPro = new List<Player>();
        public List<Player> rank5 = new List<Player>();
        public List<Player> rank4 = new List<Player>();
        public List<Player> rank3 = new List<Player>();
        public List<Player> rank2 = new List<Player>();
        public List<Player> rank1 = new List<Player>();
        public List<Player> rankMaster = new List<Player>();
        public List<Player> rankLegend = new List<Player>();

        //List of rank Lists
        public List<List<Player>> listOfRankList = new List<List<Player>>();

        Player opponent;

        public InputField inputRankA;
        public InputField inputRankB;

        public Toggle winA;
        public Button bakeButton;
        private bool canBake;

        public Text rankNameA;
        public Text rankNameB;

        private int result;

        public List<Color> rankColors = new List<Color>();

        // Equivaut au maximum de points que l'on peut perdre ou gagner en un match
        [Range(0, 10), SerializeField]
        private float k = 1.7f;

        [Range(0, 400), SerializeField]
        private int scale = 21;

        // Valeurs de retour de rankA et rankB. Ce sont des Int qui passent en String
        [HideInInspector] public float finalELOA;
        [HideInInspector] public float finalELOB;

        public Text winrateA;
        public Text winrateB;

        [Header("Nouvel Adversaire"), SerializeField]
        private OpponentDifficulty opponentDifficulty;
        [Range(0,5)]
        public float easierRange;
        [Range(0,5)]
        public float equivalentRange;
        [Range(0,5)]
        public float harderRange;

        [Header("Simulation"), SerializeField]
        int playersInit;



        // Start
        void Start()
        {
            // Initialisation pas propre des pourcentages
            float.TryParse(inputRankA.text, out float valueRankA);
            float.TryParse(inputRankB.text, out float valueRankB);
            winrateA.text = Mathf.RoundToInt(1f / (1f + Mathf.Pow(10f, (valueRankB - valueRankA) / scale)) * 100).ToString() + "%";
            winrateB.text = Mathf.RoundToInt(1f / (1f + Mathf.Pow(10f, (valueRankA - valueRankB) / scale)) * 100).ToString() + "%";

            // Initialisation du sorting des rangs
            listOfRankList.Add(rankAmateur);
            listOfRankList.Add(rankPro);
            listOfRankList.Add(rank5);
            listOfRankList.Add(rank4);
            listOfRankList.Add(rank3);
            listOfRankList.Add(rank2);
            listOfRankList.Add(rank1);
            listOfRankList.Add(rankMaster);
            listOfRankList.Add(rankLegend);

            //Ajout des joueurs virtuels et affichage du temps d'exécution
            Debug.Log("A: " + Time.time);
            createPlayers(playersInit);
            Debug.Log("B: " + Time.time);
        }

        // Update
        void Update()
        {
            // Si winA est activé, alors affiche Winner à l'écran (et inversement)
            if (winA.isOn == true)
            {
                winA.GetComponentInChildren<Text>().text = "Winner";
            }
            else
            {
                winA.GetComponentInChildren<Text>().text = "Loser";
            }

            Ranking();
        }

        public void Ranking()
        {
            //Affiche l'ELO de chaque joueur à l'écran
            if (float.TryParse(inputRankA.text, out float valueRankA) && float.TryParse(inputRankB.text, out float valueRankB))
            {
                winrateA.text = Mathf.RoundToInt(1f / (1f + Mathf.Pow(10f, (valueRankB - valueRankA) / scale)) * 100).ToString() + "%";
                winrateB.text = Mathf.RoundToInt(1f / (1f + Mathf.Pow(10f, (valueRankA - valueRankB) / scale)) * 100).ToString() + "%";

                //Les ELO ne peuvent pas dépasser 100
                valueRankA = Mathf.Clamp(valueRankA, 0f, 100f);
                if (valueRankA == 100f)
                {
                    inputRankA.text = "100";
                }

                valueRankB = Mathf.Clamp(valueRankB, 0f, 100f);
                if (valueRankB == 100f)
                {
                    inputRankB.text = "100";
                }

                //Si on appuie sur le bouton Bake, cette fonction est lancée
                if (canBake == true)
                {
                    Bake(valueRankA, valueRankB);
                    canBake = false;
                }
            }
            //////////////////////////////
            opponent = GetNewOpponent(valueRankA);
        }


        //IL EST IMPORTANT DE NOTER QUE LES POURCENTAGES DE VICTOIRE SONT ICI ARBITRAIRES ET NE REFLETENT PAS FORCEMENT LA REALITE.
        public void Bake(float vA, float vB)
        {
            int aWin = 0;
            int bWin = 0;

            // Change la variable de victoire en fonction de si winA est activé ou non
            switch (winA.isOn)
            {
                case true:
                    aWin = 1;
                    bWin = 0;
                    break;

                case false:
                    aWin = 0;
                    bWin = 1;
                    break;
            }
            
            //Met à jour l'ELO avec le calcul officiel puis l'affiche à l'écran
            finalELOA = vA + k * (aWin - (1f / (1f + Mathf.Pow(10f, (vB - vA) / scale))));
            finalELOB = vB + k * (bWin - (1f / (1f + Mathf.Pow(10f, (vA - vB) / scale))));
            finalELOA = Mathf.Round(finalELOA * 10) / 10;
            finalELOB = Mathf.Round(finalELOB * 10) / 10;
            inputRankA.text = finalELOA.ToString();
            inputRankB.text = finalELOB.ToString();
            SetRankNames(finalELOA, rankNameA, inputRankA);
            SetRankNames(finalELOB, rankNameB, inputRankB);
        }

        //Affiche le nom des rangs ainsi que leur couleur en fonction de l'ELO
        public void SetRankNames(float eloToModify, Text textToModify, InputField eloColor)
        {
            ColorBlock cb = eloColor.colors;

            if (eloToModify >= 0 && eloToModify < 20f)
            {
                cb.normalColor = rankColors[0];
                eloColor.colors = cb;
                textToModify.text = "Amateur";
            }
            else if (eloToModify >= 20f && eloToModify < 30f)
            {
                cb.normalColor = rankColors[1];
                eloColor.colors = cb;
                textToModify.text = "Professional";
            }
            else if (eloToModify >= 30f && eloToModify < 40f)
            {
                cb.normalColor = rankColors[2];
                eloColor.colors = cb;
                textToModify.text = "5th League";
            }
            else if (eloToModify >= 40f && eloToModify < 50f)
            {
                cb.normalColor = rankColors[3];
                eloColor.colors = cb;
                textToModify.text = "4th League";
            }
            else if (eloToModify >= 50f && eloToModify < 60f)
            {
                cb.normalColor = rankColors[4];
                eloColor.colors = cb;
                textToModify.text = "3rd League";
            }
            else if (eloToModify >= 60 && eloToModify < 70f)
            {
                cb.normalColor = rankColors[5];
                eloColor.colors = cb;
                textToModify.text = "2nd League";
            }
            else if (eloToModify >= 70f && eloToModify < 80f)
            {
                cb.normalColor = rankColors[6];
                eloColor.colors = cb;
                textToModify.text = "1st League";
            }
            else if (eloToModify >= 80f && eloToModify < 90f)
            {
                cb.normalColor = rankColors[7];
                eloColor.colors = cb;
                textToModify.text = "Master";
            }
            else if (eloToModify >= 90f && eloToModify <= 100f)
            {
                cb.normalColor = rankColors[8];
                eloColor.colors = cb;
                textToModify.text = "Legend";
            }

            //Pas propre - A optimiser
            if(eloToModify >= 90f)
            {
                eloColor.transform.Find("ELOValue").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                eloColor.transform.Find("ELOValue").GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            }
        }

        //Autorise le Bake
        public void CanBake()
        {
            canBake = true;
        }

        public Player GetNewOpponent(float thisPlayerELO)
        {
            Player opponent;
            float findOpponentOfElo = 0f;

            switch (opponentDifficulty)
            {
                case OpponentDifficulty.Easier:
                    findOpponentOfElo = Random.Range(thisPlayerELO - equivalentRange - easierRange, thisPlayerELO - equivalentRange);
                    //Debug.Log(findOpponentOfElo);
                    break;

                case OpponentDifficulty.Equivalent:
                    findOpponentOfElo = Random.Range(thisPlayerELO - equivalentRange, thisPlayerELO + equivalentRange);
                    //Debug.Log(findOpponentOfElo);
                    break;

                case OpponentDifficulty.Harder:
                    findOpponentOfElo = Random.Range(thisPlayerELO + equivalentRange, thisPlayerELO + equivalentRange + harderRange);
                    //Debug.Log(findOpponentOfElo);
                    break;
            }

            opponent = listOfRankList[1][2];

            return opponent;
        }



        public void WinStreakCounter()
        {


        }

        //Création et initialisation d'un nombre défini de joueurs virtuels
        public void createPlayers(int nbPlayers)
        {
            for (int i = 0; i < nbPlayers; i++)
            {
                Player playerTmp = new Player();
                playerTmp.ELO = Random.Range(0, 100);
                listPlayers.Add(playerTmp);

                SortPlayerInRankList(playerTmp);
                GeneratePlayerName(playerTmp);
            }
        }

        public void SortPlayerInRankList(Player player)
        {
            //Si la liste contient le joueur, alors le retire (puisqu'il sera réaffecté par la suite)
            if(listOfRankList[player.currentListIndex].Contains(player))
            {
                listOfRankList[player.currentListIndex].Remove(player);
            }

            //Choix de la liste dans laquelle mettre le joueur en fonction de son ELO
            if (player.ELO >= 0 && player.ELO < 20f)
            {
                player.currentListIndex = 0;
                player.rankNames = Player.RankNames.Amateur;
            }
            else if (player.ELO >= 20f && player.ELO < 30f)
            {
                player.currentListIndex = 1;
                player.rankNames = Player.RankNames.Pro;
            }
            else if (player.ELO >= 30f && player.ELO < 40f)
            {
                player.currentListIndex = 2;
                player.rankNames = Player.RankNames.FifthLeague;
            }
            else if (player.ELO >= 40f && player.ELO < 50f)
            {
                player.currentListIndex = 3;
                player.rankNames = Player.RankNames.FourthLeague;
            }
            else if (player.ELO >= 50f && player.ELO < 60f)
            {
                player.currentListIndex = 4;
                player.rankNames = Player.RankNames.ThirdLeague;
            }
            else if (player.ELO >= 60 && player.ELO < 70f)
            {
                player.currentListIndex = 5;
                player.rankNames = Player.RankNames.SecondLeague;
            }
            else if (player.ELO >= 70f && player.ELO < 80f)
            {
                player.currentListIndex = 6;
                player.rankNames = Player.RankNames.FirstLeague;
            }
            else if (player.ELO >= 80f && player.ELO < 90f)
            {
                player.currentListIndex = 7;
                player.rankNames = Player.RankNames.Master;
            }
            else if (player.ELO >= 90f && player.ELO <= 100f)
            {
                player.currentListIndex = 8;
                player.rankNames = Player.RankNames.Legend;
            }

            //Ajout du joueur dans la liste sélecitonnée
            listOfRankList[player.currentListIndex].Add(player);
        }

        //Génère un nom aléatoire au joueur
        public void GeneratePlayerName(Player player)
        {
            string[] randomLetter = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

            string nameCompFirst = randomLetter[Random.Range(0, randomLetter.Length)].ToString();
            string nameCompSecond = randomLetter[Random.Range(0, randomLetter.Length)].ToString();
            string nameCompThird = randomLetter[Random.Range(0, randomLetter.Length)].ToString();
            string nameCompFourth = randomLetter[Random.Range(0, randomLetter.Length)].ToString();
            //string nameCompFifth = randomLetter[Random.Range(0, randomLetter.Length)].ToString();

            //Il y a environ 1 chance sur 4.112.784 d'avoir deux joueurs avec le même nom
            player.pseudo = nameCompFirst + nameCompSecond + nameCompThird + nameCompFourth + "_" + (player.currentListIndex + 1);
            //Debug.Log(player.pseudo);           
        }

        //Génère un statut de connexion aléatoire au joueur
        public void GenerateOnlineStatus(Player player)
        {
            if (Random.value >= 0.5)
            {
                player.isOnline = true;
            }
            else
            {
                player.isOnline = false;
            }
        }

    }
    enum OpponentDifficulty { Easier, Equivalent, Harder }
}
