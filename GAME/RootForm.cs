﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAME
{
    public partial class RootForm : Form
    {
        private PrefabLevel currentLevel;

        private int numberOfLife = 5;
        private int point = 5;

        private bool isMenuShow = false;
        private int currentLevelNumber = 1;

        ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RootForm));

        public RootForm()
        {
            InitializeComponent();
        }

        private void RootForm_Load(object sender, EventArgs e)
        {
            mainMenu.Dock = DockStyle.Fill;
            levelsMenu.Dock = DockStyle.Fill;

            ToolbarClose();
            LevelsMenu_Close();
            HallOfFame_Close();
            HeartBar_Close();
            ContinueSelection_Close();
        }

        #region Quản lý Toolbar
        private void ToolbarOpen()
        {
            toolBar.Show();
            levelsMenu.Show();
        }
        private void ToolbarClose()
        {
            toolBar.Hide();
            menuPanel.Hide();
            levelsMenu.Hide();
        }

        private void btn_Menu_Click(object sender, EventArgs e)
        {
            if (menuPanel.Visible)
                isMenuShow = true;
            else
            {
                menuPanel.Show();
                isMenuShow = false;
            }

            menuTimer.Start();
        }

        private void menuTimer_Tick(object sender, EventArgs e)
        {
            if (isMenuShow)
            {
                menuPanel.Width -= 25;

                if (menuPanel.Width <= 0)
                {
                    menuPanel.Hide();
                    menuTimer.Stop();
                }
            }

            else
            {
                menuPanel.Width += 25;

                if (menuPanel.Width >= 350)
                    menuTimer.Stop();
            }
        }

        private void btn_Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Quản lý MainMenu
        private void MainMenu_Load(object sender, EventArgs e)
        {
            mainMenu.Dock = DockStyle.Fill;
        }


        private void mainMenu_ChoiluonButtonClick(object sender, EventArgs e)
        {
            mainMenu.Hide();

            ToolbarOpen();
            LevelsMenu_Open();
        }

        private void mainMenu_ChoitiepButtonClick(object sender, EventArgs e)
        {

        }

        private void mainMenu_CachchoiButtonClick(object sender, EventArgs e)
        {

        }

        private void btn_LevelMenu_Click(object sender, EventArgs e)
        {
            ToolbarOpen();
            LevelsMenu_Open();
            //  ContinueSelection vẫn hiện
            try
            {
                if (currentLevel != null)
                    Controls.Remove(currentLevel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            btn_Menu_Click(sender, e);
        }
        #endregion

        #region Màn chơi
        private void RightAnswer(object sender, EventArgs e)
        {
            hallOfFame.SetLevelLabel(currentLevelNumber);
            hallOfFame.Show();
        }

        private void WrongAnswer(object sender, EventArgs e)
        {
            numberOfLife--;
            heartBar.SetHearts(numberOfLife);

            Controls.Remove(currentLevel);
            level_Selection();

            if (numberOfLife <= 0)
            {
                ContinueSelection_Open();
            }
        }

        private void levelsMenu_Level_ButtonClick(object sender, EventArgs e)
        {
            LevelsMenu tempLevelsMenu = (LevelsMenu)sender;
            currentLevelNumber = tempLevelsMenu.selectedLevel;

            level_Selection();
        }

        private void level_Selection()
        {
            switch (currentLevelNumber)
            {
                case 1:
                    currentLevel = new Level1();
                    break;
                case 2:
                    currentLevel = new Level2();
                    break;
                case 3:
                    currentLevel = new Level3();
                    break;
                case 4:
                    currentLevel = new Level4();
                    break;
                //case 5:
                //    currentLevel = new Level5();
                //    break;
                //case 6:
                //    currentLevel = new Level6();
                //    break;

                default:
                    break;
            }

            level_Setup();
        }

        private void level_Setup()
        {
            LevelsMenu_Close();
            HeartBar_Open();
            heartBar.SetHearts(numberOfLife);

            btn_Suggestion.Show();

            try
            {
                currentLevel.BackColor = Color.Transparent;

                resources.ApplyResources(currentLevel, "currentLevel");
                currentLevel.Name = "currentLevel";
                Controls.Add(currentLevel);

                currentLevel.rightAnswer += new EventHandler(RightAnswer);
                currentLevel.wrongAnswer += new EventHandler(WrongAnswer);

                //  Cài đặt gợi ý cho suggestionTable
                suggestionTable.SetSuggestionText(currentLevel.SuggestionText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK);
            }

        }

        #region Quản lý gợi ý
        private void btn_Suggestion_MouseHover(object sender, EventArgs e)
        {
            btn_Suggestion.Image = Properties.Resources.suggestion;
        }

        private void btn_Suggestion_MouseLeave(object sender, EventArgs e)
        {
            btn_Suggestion.Image = Properties.Resources.suggestion_nonActive1;
        }

        private void btn_Suggestion_Click(object sender, EventArgs e)
        {
            suggestionTable.Show();
        }

        /// <summary>
        /// Trừ điểm khi chọn gợi ý
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void suggestionTable_PayToSuggestion(object sender, EventArgs e)
        {
            if (point >= 5)
            {
                point -= 5;

                suggestionTable.panel_SuggestionText_Show();
            }
            else
            {
                notificationTimer.Start();
            }
        }

        /// <summary>
        /// Hiệu ứng làm mờ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notificationTimer_Tick(object sender, EventArgs e)
        {
            label_Notification.Location.Y += 3;
        }
        #endregion

        #endregion

        #region Quản lý LevelsMenu
        //  Mở table màn chơi
        private void LevelsMenu_Open()
        {
            levelsMenu.Show();
        }

        //  Đóng table màn chơi
        private void LevelsMenu_Close()
        {
            levelsMenu.Hide();
        }
        #endregion

        #region Quản lý bảng HallOfFame
        private void HallOfFame_Open()
        {
            hallOfFame.Show();
        }

        private void HallOfFame_Close()
        {
            hallOfFame.Hide();
        }

        private void hallOfFame_LevelMenuOpen(object sender, EventArgs e)
        {
            HallOfFame_Close();
            LevelsMenu_Open();
        }

        private void hallOfFame_NextLevel(object sender, EventArgs e)
        {
            currentLevelNumber++;

            Controls.Remove(currentLevel);
            level_Selection();

            HallOfFame_Close();
        }

        private void hallOfFame_PlayAgain(object sender, EventArgs e)
        {
            Controls.Remove(currentLevel);
            level_Selection();

            HallOfFame_Close();
        }
        #endregion

        #region Quản lý HeartBar
        private void HeartBar_Open()
        {
            heartBar.Show();
        }

        private void HeartBar_Close()
        {
            heartBar.Hide();
        }
        #endregion

        #region Quản lý ContinueSelection
        private void ContinueSelection_Open()
        {
            continueSelection.Show();
            currentLevel.Enabled = false;
        }

        private void ContinueSelection_Close()
        {
            continueSelection.Hide();
        }
        #endregion

    }
}
