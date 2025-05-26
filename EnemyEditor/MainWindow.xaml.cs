using Newtonsoft.Json;
using Rogue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EnemyEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Lisää vihollisen listaan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEnemyToList(object sender, RoutedEventArgs e)
        {
            // Get the contents of TextBox elements
            string name = EnemyNameEntry.Text;
            string hpString = EnemyHitpointsEntry.Text;
            string spriteIndex = EnemySpriteEntry.Text;

            // TryParse returns true if hpString can be converted to int
            int hpInt;
            int spriteInt;
            if(Int32.TryParse(spriteIndex, out spriteInt))
            {
                if (Int32.TryParse(hpString, out hpInt))
                {
                    // Test that the name is not empty
                    if (string.IsNullOrEmpty(name) == false)
                    {
                        // All ok: Create new Enemy and add it to ListBox
                        EnemyListBox.Items.Add(new Rogue.Enemy(name, hpInt, spriteInt));
                    }
                }
            }
            
        }

        private void SaveEnemiesToJSON(object sender, RoutedEventArgs e)
        {
            List<Rogue.Enemy> tempList = new List<Rogue.Enemy>();

            for (int i = 0; i < EnemyListBox.Items.Count; i++)
            {
                // Muuta ListBox elementissä oleva Object tyyppinen olio Enemy tyyppiseksi
                Rogue.Enemy enemy = (Rogue.Enemy)EnemyListBox.Items[i];
                // Lisää vihollinen listaan
                tempList.Add(enemy);

            }

            string jsonString = JsonConvert.SerializeObject(tempList);

            File.WriteAllText(("EnemyTiedot.txt"), jsonString);
        }
    }
}
