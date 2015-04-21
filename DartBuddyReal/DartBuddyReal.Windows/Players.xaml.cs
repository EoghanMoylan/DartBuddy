using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Players : Page
    {
        List<GlobalVars> players = new List<GlobalVars>();
        ApplicationDataContainer listOfPlayersLocal = ApplicationData.Current.LocalSettings;
        string playerOneName = "DefaultOne";
        string playerTwoName = "DefaultTwo";
        GlobalVars currentPlayer;
        DataTransferManager myDataManager =
        DataTransferManager.GetForCurrentView();
       
        public Players()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            gvPlayerDetails.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            myDataManager.DataRequested += myDataManager_DataRequested;
            try 
            {
                await LoadThemIn();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            string[] lastMatchDetails = e.Parameter as string[];
            sortItOut(lastMatchDetails);
            base.OnNavigatedTo(e);
        }
        public void sortItOut(string [] lastMatchDetails)
        {
            for (int i = 0; i < players.Count(); i++)
            {
                if (lastMatchDetails[0] == players[i].playerName)
                {
                    players[i].playerSum += Int32.Parse(lastMatchDetails[1]);
                    players[i].playerDartsThrown += Int32.Parse(lastMatchDetails[2]);

                    if(players[i].playerDartsThrown != 0)
                    {
                        players[i].playerAverage =(Double)(players[i].playerSum / players[i].playerDartsThrown)*3;
                    }
 
                    players[i].playerWins += Int32.Parse(lastMatchDetails[3]);

                }
                else if(lastMatchDetails[4]==players[i].playerName)
                {
                    players[i].playerSum += Int32.Parse(lastMatchDetails[5]);
                    players[i].playerDartsThrown += Int32.Parse(lastMatchDetails[6]);
                    if (players[i].playerDartsThrown != 0)
                    {
                        players[i].playerAverage =(Double)(players[i].playerSum / players[i].playerDartsThrown)*3;
                    }
                    players[i].playerWins += Int32.Parse(lastMatchDetails[7]);
                }
            }
        }
        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            await SaveThem();
            base.OnNavigatedFrom(e);
        }
        private async void RefreshList()
        {
          await  SaveThem();
          await LoadThemIn();
        }
        private void HowToPlay_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(HowToPlay));
        }

        private void Game_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string [] playersOneAndTwo = { playerOneName, playerTwoName };
            Frame.Navigate(typeof(MainPage),playersOneAndTwo);
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            GlobalVars newPlayer = new GlobalVars();
            newPlayer.playerName = txtNewPlayerName1.Text;
            newPlayer.playerAverage = 0;
            newPlayer.playerWins = 0;
            if (CheckIfPlayerExists(newPlayer.playerName) == false)
            {
                players.Add(newPlayer);
            }
            RefreshList();       
        }
        private bool CheckIfPlayerExists(string newPlayer)
        {
            bool doesitExist = false;
            for (int i = 0; i < players.Count(); i++)
            {
                if(newPlayer == players[i].playerName)
                {
                    txtaddnewTitle.Text = "Cannot use same name twice";
                    doesitExist = true;   
                }
            }
                return doesitExist;
        }

        private void viewPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void btnPlayerOne_Tapped(object sender, TappedRoutedEventArgs e)
        {
            playerOneName = txtPlayerName.Text;
        }

        private void btnPlayerTwo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            playerTwoName = txtPlayerName.Text;
        }
        private void viewPlayers_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
        }
        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StackPanel clickedItem = (StackPanel)sender;
   
            int myIndex = -1;

            for (int i = 0; i < players.Count(); i++)
            {
                if (players[i].playerName != null)
                {
                    if (clickedItem.Tag != null)
                    {
                        if (clickedItem.Tag.ToString() == players[i].playerName)
                        {
                            myIndex = i;
                            currentPlayer = players[i];
                            break;
                        }
                    }
                }
            }
            if (myIndex == -1)
            {
                return;
            }

            txtPlayerName.Text = players[myIndex].playerName;
            txtPlayerWins.Text = players[myIndex].playerWins.ToString();
            txtPlayerAverage.Text = players[myIndex].playerAverage.ToString();

            if (gvPlayerDetails.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                gvPlayerDetails.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }
        public async Task LoadThemIn()
        {
            StorageFile localFile;
            try
            {
                localFile = await ApplicationData.Current.LocalFolder.GetFileAsync("localData.txt");
            }
            catch (FileNotFoundException ex)
            {
                string error = ex.Message;
                localFile = null;
            }
            if (localFile != null)
            {
                string localData = await FileIO.ReadTextAsync(localFile);


                players = ObjectSerializer<List<GlobalVars>>.FromXml(localData);
            }
            viewPlayers.ItemsSource = players;  
        }
        public async Task SaveThem()
        {
            try
            {
                string localData = ObjectSerializer<List<GlobalVars>>.ToXml(players);

                if (!string.IsNullOrEmpty(localData))
                {
                    StorageFile localFile = await ApplicationData.Current.LocalFolder.
                        CreateFileAsync("localData.txt", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(localFile, localData);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private async void btnRemovePlayer_Tapped(object sender, TappedRoutedEventArgs e)
        {
           players.Remove(currentPlayer);
           await SaveThem();
           await LoadThemIn();
        }
        void myDataManager_DataRequested(DataTransferManager sender,
                         DataRequestedEventArgs args)
        {
            // do work for set up of datapackage object here
            // title  - required
            DataPackage myData = args.Request.Data;
            myData.Properties.Title = "Player stats";
            myData.Properties.Description = "Check out how these player stats";
            string messageText;

            if (currentPlayer != null)
            {
                messageText = (currentPlayer.playerName + "<br>Player Average : " + currentPlayer.playerAverage.ToString() + "<br>Player Wins : " + currentPlayer.playerWins.ToString());
            }
            else
            {
                messageText = ("Please Select A player");
            }
            string localImage = "ms-appx:///Assets/SplashScreen.scale-100.png";
            string htmltemplate = "<p> " + messageText + "</p><p>Brought to you by: <img src=\"" + localImage + "\">.</p>";
            string htmlFinal = HtmlFormatHelper.CreateHtmlFormat(htmltemplate);
            myData.SetHtmlFormat(htmlFinal);
            RandomAccessStreamReference imageStream = RandomAccessStreamReference.CreateFromUri(new Uri(localImage));

            myData.ResourceMap[localImage] = imageStream;
        }

        private void About_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
        }


}
