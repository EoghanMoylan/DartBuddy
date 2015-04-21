using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DartBuddyReal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int gameType = 501;
        public int playerOneCurrentScore;
        public int playerTwoCurrentScore;
        public int thisRoundScore = 0;
        public int setLimit = 6;
        public int dartCount = 3;
        public int playerOneSetScore = 0;
        public int playerTwoSetScore = 0;
        public int tempPoneScore = 0;
        public int tempPTwoScore = 0;
        public   int collectiveDartHit = 0;
        public bool isPlayerOne = true;
        public string playerOneName = "Default 1";
        public string playerTwoName = "Default 2";
        public int playerOneTotals =1;
        public int playerTwoTotals =1;
        public int playerOneDartsThrown= 0;
        public int playerTwoDartsThrown= 0;
        public int playerOneWins =0;
        public int playerTwoWins=0;
        DataTransferManager myDataManager =
         DataTransferManager.GetForCurrentView();


        public MainPage()
        {
            this.InitializeComponent();
            resetAll();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] playersOneAndTwo;
            // when the page is loaded, register the event handler
            myDataManager.DataRequested += myDataManager_DataRequested;
            playersOneAndTwo = e.Parameter as string[];
            if (playersOneAndTwo != null)
            {
                playerOneName = playersOneAndTwo[0];
                playerTwoName = playersOneAndTwo[1];
                txtPlayerOne.Text = playerOneName;
                txtPlayerTwo.Text = playerTwoName;
            }
            base.OnNavigatedTo(e);
        }


        private void txtPlayerOne_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private void resetAll()
        {
            txtPlayerOne.Text = playerOneName;
            txtPlayerTwo.Text = playerTwoName;
            playerOneCurrentScore = gameType;
            playerTwoCurrentScore = gameType;
            txtPlayerOneScore.Text = playerOneCurrentScore.ToString();
            txtPlayerTwoScore.Text = playerTwoCurrentScore.ToString();
            PlayerOneIndicator.Opacity = 100;
            PlayerTwoIndicator.Opacity = 0;
            playerTwoSetScore = 0;
            playerOneSetScore = 0;
            thisRoundScore = 0;
            txtThisRoundCurrScore.Text = thisRoundScore.ToString() ;
            txtPlayerTwoSetScore.Text = playerTwoSetScore.ToString();
            txtPlayerOneSetScore.Text = playerOneSetScore.ToString();
            tempPoneScore = gameType;
            tempPTwoScore = gameType;
            dartCount = 3;
            collectiveDartHit = 0;
        }
        private string getListOfFinishes(int score)
        {
            string listOfFinshes = "";
           
            if (score <= 170)
            {
                  for(int j = 20 ; j>=1; j--)
                  {
                      for (int i = 20; i >=1 ;i --)
                      {
                          for (int finI = 20 ; finI >=1 ; finI--)
                          {
                              //less than three dart finishes
                              if (score - (j * 2) == 0)
                              {
                                  listOfFinshes += "D" + j + "\n";
                              }
                              if (score - ((j * 3) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "T" + j + "  D" + finI + "\n";
                              }
                              if (score - ((j * 2) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "D" + j + "  D" + finI + "\n";
                              }
                              if (score - ((j) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "" + j + "  D" + finI + "\n";
                              }
                              //three dart finishes
                              if (score - ((j * 3) + (i * 3) + (finI*2)) ==0)
                              {
                                  listOfFinshes += "T" + j + "   T" + i + "  D" + finI + "\n";
                              }
                              if (score - ((j * 2) + (i * 2) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "D" + j + "   D" + i + "  D" + finI + "\n";
                              }
                              if (score - ((j * 3) + (i * 2) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "T" + j + "   D" + i + "  D" + finI + "\n";
                              }
                              if (score - ((j * 3) + (i) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "T" + j + "   " + i + "  D" + finI + "\n";
                              }
                              if (score - ((j* 2) + (i) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "D" + j + "   " + i + "  D" + finI + "\n";
                              }
                              if (score - ((j) + (i) + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "" + j + "   " + i + "  D" + finI + "\n";
                              }
                              if (score - (50 + ((i * 3) + (finI * 2))) == 0)
                              {
                                  listOfFinshes += "Bull" + "   T" + i + "  D" + finI + "\n";
                              }
                              if (score - (100 + (finI * 2)) == 0)
                              {
                                  listOfFinshes += "Bull   Bull" + "  D" + finI + "\n";
                              }
                          }
                      }
                }
            }

            return listOfFinshes;
        }
        public void updateVarious()
        {
            txtThisRoundCurrScore.Text = thisRoundScore.ToString();
            txtPlayerOneListOfFinshes.Text = getListOfFinishes(playerOneCurrentScore);
            txtPlayerTwoListOfFinshes.Text = getListOfFinishes(playerTwoCurrentScore);

            switch(dartCount)
            {
                case 3:
                    Dart1.Opacity = 100;
                    Dart2.Opacity = 100;
                    Dart3.Opacity = 100;
                    break;
                case 2:
                    Dart1.Opacity = 0;
                    break;
                case 1:
                    Dart2.Opacity = 0;
                    break;
                case 0:
                    Dart3.Opacity = 0;
                    break;
            }

            if (isPlayerOne)
            {
                txtPlayerOneListOfFinshes.Text = getListOfFinishes(playerOneCurrentScore);
                PlayerOneIndicator.Opacity = 100;
                PlayerTwoIndicator.Opacity = 0;
            }
            else
            {
                txtPlayerTwoListOfFinshes.Text = getListOfFinishes(playerTwoCurrentScore);
                PlayerOneIndicator.Opacity = 0;
                PlayerTwoIndicator.Opacity = 100;
            }
        }
        private async void updateBoard(int dartHit, bool isDouble)
        {
 
            dartCount--;
            thisRoundScore += dartHit;
            collectiveDartHit += dartHit;
            updateVarious();

            //figure out if player one can finish
            if(isPlayerOne)
            {
                playerOneDartsThrown++;

                if (!getListOfFinishes(playerOneCurrentScore).Equals("") && playerOneCurrentScore - collectiveDartHit <= 0)
                {
                    if (playerOneCurrentScore - collectiveDartHit == 0)
                    {
                        if (isDouble)
                        {
                            playerOneTotals += collectiveDartHit;
                            SetIsOver(playerOneName);
                        }
                        else
                        {
                            playerOneCurrentScore = tempPoneScore;
                        }
                    }
                    else
                    {
                        playerOneCurrentScore = tempPoneScore;
                    }
                }
                else if (playerOneCurrentScore - collectiveDartHit > 1 && dartCount == 0)
                {
                    playerOneTotals += collectiveDartHit;
                    playerOneCurrentScore -= collectiveDartHit;
                }
                else
                {
                    playerOneCurrentScore = tempPoneScore;
                }
            }
            //do the same for player two
            else
            {
                playerTwoDartsThrown++;
                if (!getListOfFinishes(playerTwoCurrentScore).Equals("") && playerTwoCurrentScore - collectiveDartHit <= 0)
                {
                    if (playerTwoCurrentScore - collectiveDartHit == 0)
                    {
                        if (isDouble)
                        {
                            playerTwoTotals += collectiveDartHit;
                            SetIsOver(playerTwoName);
                        }
                        else
                        {
                            playerTwoCurrentScore = tempPTwoScore;
                        }
                    }
                    else
                    {
                        playerTwoCurrentScore = tempPTwoScore;
                    }
                }
                else if (playerTwoCurrentScore - collectiveDartHit > 1 && dartCount == 0)
                {
                    playerTwoTotals += collectiveDartHit;
                    playerTwoCurrentScore -= collectiveDartHit;
                }
                else
                {
                    playerTwoCurrentScore = tempPTwoScore;
                }
            }

     
                if (dartCount == 0)
                {
                    dartCount = 3;
                    collectiveDartHit = 0;
                    thisRoundScore = 0;
                    await Task.Delay(TimeSpan.FromSeconds(0.5));
                    txtThisRoundCurrScore.Text = "0";
                    if (isPlayerOne)
                    {
                        txtPlayerOneScore.Text += "\n" + playerOneCurrentScore.ToString();
                        isPlayerOne = false;
                    }
                    else
                    {
                        txtPlayerTwoScore.Text += "\n" + playerTwoCurrentScore.ToString();

                        isPlayerOne = true;
                    }

                    tempPoneScore = playerOneCurrentScore;
                    tempPTwoScore = playerTwoCurrentScore;
                }

                updateVarious();
        }
        private void SetIsOver(string winner)
        {
            if(winner.Equals(playerOneName))
            {
                playerOneSetScore++;
                playerOneWins++;
            }
            else
            {
                playerTwoWins++;
                playerTwoSetScore++;
            }
            updateBoard(0, false);
            dartCount = 3;
            collectiveDartHit = 0;
            thisRoundScore = 0;
            txtThisRoundCurrScore.Text = "0";
            playerTwoCurrentScore = gameType;
            playerOneCurrentScore = gameType;
            tempPoneScore = playerOneCurrentScore;
            tempPTwoScore = playerTwoCurrentScore;
            txtPlayerTwoSetScore.Text = playerTwoSetScore.ToString();
            txtPlayerOneSetScore.Text = playerOneSetScore.ToString();
            txtPlayerTwoScore.Text = playerTwoCurrentScore.ToString();
            txtPlayerOneScore.Text = playerOneCurrentScore.ToString();


            if (playerOneCurrentScore == setLimit || playerTwoCurrentScore == setLimit)
            {
                resetAll();
            }
        }

        private void NineDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(18, true);         
        }

        private void TwentyDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(40,true);
        }

        private void OneDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(2,true);
        }

        private void EighteenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(36,true);
        }

        private void FourDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(8, true);
        }

        private void ThirteenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(26, true);
        }

        private void SixDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(12, true);
        }

        private void TenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(20, true);
        }

        private void FifteenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(30, true);
        }

        private void TwoDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(4, true);
        }

        private void SeventeenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(34, true);
        }

        private void ThreeDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(6, true);
        }

        private void NineteenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(38, true);
        }

        private void SevenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(14, true);
        }

        private void SixteenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(32, true);
        }

        private void EightDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(16, true);
        }

        private void ElevenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(22, true);
        }

        private void FourteenDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(28, true);
        }

        private void TweleveDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(24, true);
        }

        private void FiveDouble_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(10, true);
        }

        private void TwentySingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(20, false);
        }

        private void OneSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(1, false);
        }

        private void EighteenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(18, false);
        }

        private void FourSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(4, false);
        }

        private void ThirteenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(13, false);
        }

        private void TenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(10, false);
        }

        private void sixSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(6, false);
        }

        private void FifteenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(15, false);
        }

        private void TwoSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(2, false);
        }

        private void SeventeenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(17, false);
        }

        private void ThreeSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(3, false);
        }

        private void NineteenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(19, false);
        }

        private void SevenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(7, false);
        }

        private void SixteenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(16, false);
        }

        private void EightSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(8, false);
        }

        private void ElevenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(11, false);
        }

        private void FourteenSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(14, false);
        }

        private void NineSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(9, false);
        }

        private void TwelveSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(12, false);
        }

        private void FiveSingle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(5, false);
        }

        private void TweleveTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(36, false);
        }

        private void FiveTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(15, false);
        }

        private void TwentyTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(60, false);
        }

        private void OneTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(3, false);
        }

        private void EighteenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(54, false);
        }

        private void FourTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(12, false);
        }

        private void ThirteenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(39, false);
        }

        private void SixTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(18, false);
        }

        private void TenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(30, false);
        }

        private void FifteenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(45, false);
        }

        private void TwoTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(6, false);
        }

        private void SeventeenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(51, false);
        }

        private void ThreeTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(9, false);
        }

        private void NineteenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(57, false);
        }

        private void SevenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(21, false);
        }

        private void SixteenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(48, false);
        }

        private void EightTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(24, false);
        }

        private void ElevenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(33, false);
        }

        private void FourteenTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(42, false);
        }

        private void NineTriple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(27, false);
        }

        private void TheTwentyFive_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(25, false);
        }

        private void Bullseye_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(50, true);
        }
        void myDataManager_DataRequested(DataTransferManager sender,
                                 DataRequestedEventArgs args)
        {
            // do work for set up of datapackage object here
            // title  - required
            DataPackage myData = args.Request.Data;
            myData.Properties.Title = "My Current Game";
            myData.Properties.Description = "Check out how my game is going!";
            string messageText = (playerOneName + " : " + playerOneCurrentScore.ToString() + " : " + "\t" + playerTwoName + " : " + playerTwoCurrentScore.ToString()
                + "<br>"+ playerOneName + " : " + playerOneSetScore.ToString() + " : " + "    " + playerTwoName + " : " + playerTwoSetScore.ToString());
            string localImage = "ms-appx:///Assets/SplashScreen.scale-100.png";
            string htmltemplate = "<p> " + messageText + "</p><p>Brought to you by: <img src=\"" + localImage + "\">.</p>";
            string htmlFinal = HtmlFormatHelper.CreateHtmlFormat(htmltemplate);
            myData.SetHtmlFormat(htmlFinal);
            RandomAccessStreamReference imageStream = RandomAccessStreamReference.CreateFromUri(new Uri(localImage));

            myData.ResourceMap[localImage] = imageStream;
        }
        private void swGameMode_Toggled(object sender, RoutedEventArgs e)
        {
            if(gameType == 501)
            {
                gameType = 301;
                resetAll();
            }
            else
            {
                gameType = 501;
                resetAll();
            }
        }

        private void zeroHit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateBoard(0, false);
        }


        private void HowToPlay_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(HowToPlay));
        }

        private void Player_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string [] playerOneAndTwoDetails = {playerOneName, playerOneTotals.ToString(), playerOneDartsThrown.ToString(), playerOneWins.ToString(), playerTwoName, playerTwoTotals.ToString(), playerTwoDartsThrown.ToString(), playerTwoWins.ToString()};
            Frame.Navigate(typeof(Players), playerOneAndTwoDetails);
        }
        private void About_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
    }
}