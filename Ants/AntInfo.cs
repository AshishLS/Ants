using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace Ants
{
    class AntInfo
    {
        public Vector direction;
        public Button body;
        private bool oddTurn;
        public System.Windows.Point foodLocation;
        public System.Windows.Point homeLocation;
        private bool foodFound;
        private int movesInSingleDirection;
        public int howManyAntsInformed;
        private List<int> alreadyMetAnts;
        private bool goTohome = false;         

        public AntInfo(Button antBody, System.Drawing.Point homeLoc)
        {
            this.homeLocation = new System.Windows.Point((double)homeLoc.X, (double)homeLoc.Y);
            this.alreadyMetAnts = new List<int>();
            int xdir = Form1.RandomGenerator.Next(-360, 360);
            int ydir = Form1.RandomGenerator.Next(-360, 360);
            this.direction = new Vector(xdir, ydir);
            this.direction.Normalize();

            this.body = antBody;
        }

        internal void Move()
        {
            System.Windows.Point currentLoc = new System.Windows.Point(this.body.Location.X, this.body.Location.Y);
            if (this.howManyAntsInformed >= Form1.numberOfAnts - 1 || this.alreadyMetAnts.Count >= Form1.numberOfAnts - 1)
            {
                this.direction = this.DecideDirection(currentLoc);
                this.direction.Normalize();
                this.body.BackColor = Color.Green;
            }

            var newloc = Vector.Add(Vector.Multiply(Form1.AntSpeed,this.direction), currentLoc);
            if (this.oddTurn == true)
            {
                this.body.Location = new System.Drawing.Point((int)Math.Floor(newloc.X), (int)Math.Floor(newloc.Y));
            }
            else
            {
                this.body.Location = new System.Drawing.Point((int)Math.Ceiling(newloc.X), (int)Math.Ceiling(newloc.Y));
            }
            this.oddTurn = !this.oddTurn;

            this.CheckAndChangeDirection();
            this.movesInSingleDirection++;

            if (this.foodFound && this.movesInSingleDirection > 100)
            {
                this.ChangeDirection();
                this.movesInSingleDirection = 0;
            }
            //Console.WriteLine(String.Format("location: x = {0}, y = {1}", this.body.Location.X, this.body.Location.Y));
        }

        /// <summary>
        /// Depending on whether the ant is moving towards food or home, decide its direction.
        /// </summary>
        /// <param name="currentLoc"></param>
        /// <returns></returns>
        private Vector DecideDirection(System.Windows.Point currentLoc)
        {
            var foodDir = System.Windows.Point.Subtract(this.foodLocation, currentLoc);
            var homeDir = System.Windows.Point.Subtract(this.homeLocation, currentLoc);
            if (foodDir.Length < 10)
            {
                goTohome = true;
            }
            else if (homeDir.Length < 10)
            {
                goTohome = false;
            }
            Vector returndir = goTohome ? homeDir : foodDir;
            //Console.WriteLine(String.Format("RETURN direction: X = {0}, Y = {1}", returndir.X, returndir.Y));
            return returndir;
        }

        internal void CheckAndChangeDirection()
        {
            int newx = this.body.Location.X;
            int newy = this.body.Location.Y;
            bool changeDirection = false;

            // Move them inside by 10 pixels.
            if (this.body.Location.X > Form1.formWidth)
            {
                newx -= 10;
                changeDirection = true;
            }
            if (this.body.Location.Y > Form1.formHeight)
            {
                newy -= 10;
                changeDirection = true;
            }
            if (this.body.Location.X < 0)
            {
                newx += 10;
                changeDirection = true;
            }
            if (this.body.Location.Y < 0)
            {
                newy += 10;
                changeDirection = true;
            }

            if (changeDirection == true)
            {
                this.body.Location = new System.Drawing.Point(newx, newy);
                this.ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            // Change direction.
            int xdir = Form1.RandomGenerator.Next(-360, 360);
            int ydir = Form1.RandomGenerator.Next(-360, 360);
            this.direction = new Vector(xdir, ydir);
            this.direction.Normalize();
        }

        internal void CheckClashWithOtherAnt(AntInfo otherAnt)
        {
            int maxOfTwo = Math.Max(this.howManyAntsInformed, otherAnt.howManyAntsInformed);
            this.howManyAntsInformed = maxOfTwo;
            otherAnt.howManyAntsInformed = maxOfTwo;

            if (!this.alreadyMetAnts.Contains(otherAnt.GetHashCode()) && otherAnt.foodFound)
            {
                this.alreadyMetAnts.Add(otherAnt.GetHashCode());

                maxOfTwo = Math.Max(this.howManyAntsInformed, this.alreadyMetAnts.Count);
                this.howManyAntsInformed = maxOfTwo;
                otherAnt.howManyAntsInformed = maxOfTwo;
            }
            if (this.foodFound && otherAnt.foodFound) // if both know the food, don't proceed.
            {
                return;
            }
            if (!this.foodFound && !otherAnt.foodFound)// if both don't know the food, don't proceed
            {
                return;
            }
            bool clashing = CheckClash(otherAnt.body);

            if (clashing)
            {
                if (this.foodFound)
                {
                    otherAnt.foodLocation = this.foodLocation;
                    otherAnt.foodFound = true;
                    otherAnt.body.BackColor = Color.Red;
                    otherAnt.howManyAntsInformed++;
                }
                else
                {
                    this.foodLocation = otherAnt.foodLocation;
                    this.foodFound = true;
                    this.body.BackColor = Color.Red;
                    this.howManyAntsInformed++;
                }
                this.alreadyMetAnts.Add(otherAnt.GetHashCode());
                this.ChangeDirection();
            }
        }

        private bool CheckClash(Button otherBody)
        {
            bool clashing = false;
            int minX = otherBody.Location.X;
            int minY = otherBody.Location.Y;
            int maxX = otherBody.Location.X + this.body.Width;
            int maxY = otherBody.Location.Y + this.body.Height;

            int otherAntminX = this.body.Location.X;
            int otherAntminY = this.body.Location.Y;
            int otherAntmaxX = this.body.Location.X + this.body.Width;
            int otherAntmaxY = this.body.Location.Y + this.body.Height;

            if (minX > otherAntminX && minX < otherAntmaxX
                && minY > otherAntminY && minY < otherAntmaxY)
            {
                clashing = true;
            }
            else if (maxX < otherAntmaxX && maxX > otherAntminX
                && maxY < otherAntmaxY && maxY > otherAntminY)
            {
                clashing = true;
            }
            return clashing;
        }

        internal void CheckClashWithFood(Button button)
        {
            if (this.foodFound || button == null)
                return;

            bool clashing = this.CheckClash(button);
            if (clashing)
            {
                this.foodLocation = new System.Windows.Point(button.Location.X, button.Location.Y);
                this.foodFound = true;
                this.body.BackColor = Color.Red;
            }
        }
    }
}
