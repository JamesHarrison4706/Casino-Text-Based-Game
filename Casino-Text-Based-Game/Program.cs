using System;
using System.IO; // files
using System.Threading; // program delays
using System.Diagnostics; // timer
using System.Collections.Generic; // lists

namespace TextBasedGame
{
    //----------ENUMS----------

    // Different possible user input types
    enum UserInput
    {
        Confirm,
        Disconfirm,
        ValidBet,
        ContainsFirstWords,
        ContainsSecondWords,
        ContainsThirdWords,
        Null
    }



    //---------------ROOMS--------------

    static class Casino
    {
        public static void StartUp(Player player)
        {
            Console.WriteLine("-Outside Casino-\n" +
                              "You are standing outside the newly established Casino.\n" +
                              "The great doors stand tall, wide open ahead, enticing you to go in.\n" +
                              "Do you enter?");

            UserInput enterCasino = Program.InputCheck(player, question: true);

            if (enterCasino == UserInput.Disconfirm)
            {
                Console.WriteLine("See you soon then! You can always return when you are ready to gamble.");
                Environment.Exit(0);
            }
            else if (enterCasino == UserInput.Confirm)
            {
                Program.timer.Start(); // Begins stopwatch for elapsed time

                Entrance(player);
            }
        }

        private static void Entrance(Player player)
        {
            // Offering player map of casino
            if (!player.items.Contains("map"))
            {
                Console.WriteLine("-Casino Entrance-\n" +
                                  "Immediately upon arriving at the doors you are greeted by one of the casino's staff offering you a map of the place.\n" +
                                  "They highly reccomend taking the map and tell you it makes navigation much easier.\n" +
                                  "Do you take it?");
                UserInput takeMap = Program.InputCheck(player, question: true);

                if (takeMap == UserInput.Disconfirm)
                {
                    Console.WriteLine("Despite the worker's advice, you decline the map.\n");
                }
                else if (takeMap == UserInput.Confirm)
                {
                    Console.WriteLine("You accept the map and can now view it at any time by typing \"open map\".\n");
                    player.AddItem("map");
                }
            }

            Inside(player);
        }

        public static void Inside(Player player)
        {
            // Random chance that when player has no more money they are given some
            Random rnd = new Random();
            if (player.money == "0" && rnd.Next(3) == 0)
            {
                int donation = rnd.Next(5, 11);

                Thread.Sleep(1000);
                Console.WriteLine("Someone notices you looking quite upset and asks you what is wrong.\n" +
                                  "You tell them how you have no more money to spend.\n" +
                                  $"Fortunately, out of the kindness of their heart, they decide to lend you £{donation}\n");
                Thread.Sleep(4000);

                player.money = donation.ToString();
            }

            Console.WriteLine("-Centre Of Casino-\n" +
                              "You are now standing in the centre of the casino, surrounded by opportunity.\n" +
                              "As you look around you can see a bar, a shop, a VIP lounge and a variety of games like blackjack, roulette and slots.");
            Program.InputCheck(player, room: true);
        }

    }

    static class StaffOnly
    {
        public static void StartUp()
        {
            Console.WriteLine("You cannot access this room!");
        }

        public static void Inside(Player player)
        {
            Console.WriteLine("You feel yourself being tied to a chair before the bag is finally lifted over your head.\n" +
                              "The casino staff beat you senseless saying this casino does not allow cheats!\n");
            Thread.Sleep(3000);

            // Random chance of dying versus getting compensation
            Random rnd = new Random();
            if (rnd.Next(3) == 0)
            {
                Console.WriteLine("You are beaten to death and die.\n\nGAME OVER");
                player.EraseData();
                Environment.Exit(0);
            }

            int compensation = rnd.Next(50, 101);

            Console.WriteLine("Then, all of a sudden, someone bursts in the room saying your snail was found to be perfectly normal.\n" +
                              "In fact, they say, the snail was purchased in this very casino.\n" +
                              $"The people instantly stop beating you. They apologise and decide to give you £{compensation} compensation.\n");
            player.money = (int.Parse(player.money) + compensation).ToString();

            Thread.Sleep(5000);
            Casino.Inside(player);
        }
    }

    static class Bar
    {
        public static void StartUp(Player player)
        {
            Console.WriteLine("-BAR-");

            // Bar fight for VIP pass
            if (!player.items.Contains("VIP pass"))
            {
                Console.WriteLine("You walk over to the entrance of the bar and sit yourself down on a stool by the long counter.\n" +
                                  "But before you can even order an angry man stumbles toward you shouting something.\n" +
                                  "He claims you cheated him many years ago, before swinging at you.\n");
                Thread.Sleep(4000);

                if (player.items.Contains("baseball bat"))
                {
                    Console.WriteLine("Luckily you are prepared, you pull out your baseball bat and start beating the man.\n" +
                                      "You knock him unconscious and notice he has dropped something - it looks to be some sort of VIP pass.\n" +
                                      "You go over to pick it up before leaving the bar.\n");
                    Thread.Sleep(4000);
                    player.AddItem("VIP pass");
                    Casino.Inside(player);
                }
                else
                {
                    Console.WriteLine("You are beaten to death and die.\n\nGAME OVER");
                    player.EraseData();
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("You walk over to the entrance of the bar but notice that it is closed off.\n" +
                                  "Someone goes to you and tells you that apparently a bar fight broke out recently.\n" +
                                  "Now the bar is closed for further inspection.\n");
                Thread.Sleep(3000);
                Casino.Inside(player);
            }
        }
    }

    static class Shop
    {
        public static void StartUp(Player player)
        {
            string[] snail = new string[] { " snail " };
            string[] bat = new string[] { " baseball bat ", " bat " };
            string[] telephone = new string[] { " telephone ", " phone " };
            string[] buy = new string[] { " buy ", " get ", " pay for " };

            Console.WriteLine("-SHOP-\n" +
                              "You make your way into the small shop built into the casino.\n" +
                              "Despite the grandiose appearance of the area, there only appears to be three things for sale:\n" +
                              "A snail (£20), a baseball bat (£40), and a telephone (£80)");

            while (true)
            {
                UserInput choice = Program.InputCheck(player, snail, buy, bat, buy, telephone, buy, room: true);

                // Buying snail
                if (choice == UserInput.ContainsFirstWords)
                {
                    if (player.items.Contains("snail"))
                    {
                        Console.WriteLine("You already own a snail.");
                    }
                    else if (int.Parse(player.money) >= 20)
                    {
                        player.AddItem("snail");
                        player.money = (int.Parse(player.money) - 20).ToString();
                        Console.WriteLine("You just bought a snail. I wonder when this could be useful.");
                    }
                    else
                    {
                        Console.WriteLine("You do not have enough money for a snail at this time.");
                    }
                }
                // Buying baseball bat
                else if (choice == UserInput.ContainsSecondWords)
                {
                    if (player.items.Contains("baseball bat"))
                    {
                        Console.WriteLine("You already own a baseball bat.");
                    }
                    else if (int.Parse(player.money) >= 40)
                    {

                        player.AddItem("baseball bat");
                        player.money = (int.Parse(player.money) - 40).ToString();
                        Console.WriteLine("You just bought a baseball bat. Might come in handy if you ever get into a fight.");
                    }
                    else
                    {
                        Console.WriteLine("You do not have enough money for a baseball bat at this time.");
                    }
                }
                // Buying telephone
                else if (choice == UserInput.ContainsThirdWords)
                {
                    if (player.items.Contains("telephone"))
                    {
                        Console.WriteLine("You already own a telephone.");
                    }
                    else if (int.Parse(player.money) >= 80)
                    {
                        player.AddItem("telephone");
                        player.money = (int.Parse(player.money) - 80).ToString();
                        Console.WriteLine("You just bought a telephone. But who should you call?");
                    }
                    else
                    {
                        Console.WriteLine("You do not have enough money for a telephone at this time.");
                    }
                }
            }
        }
    }

    static class VIPlounge
    {
        public static void StartUp(Player player)
        {
            // Check to see whether player can access VIP lounge
            if (player.items.Contains("VIP pass"))
            {
                Console.WriteLine("You walk over to the VIP Lounge.\n" +
                                  "Before the bouncer can turn you away you smugly pull out a VIP pass.\n" +
                                  "He looks at you suspiciously for a while before letting you in.\n");
                Thread.Sleep(3000);
                Inside(player);
            }
            else
            {
                Console.WriteLine("You head towards the VIP Lounge, intrigued by the people who might be inside.\n" +
                                  "Unfortunately, to your dismay, the bouncer turns you away, saying next time come back with a pass.\n");
                Thread.Sleep(3000);
                Casino.Inside(player);
            }
        }

        public static void Inside(Player player)
        {
            string[] go = { " go " };
            string[] left = { " left ", " crowd ", " people " };
            string[] right = { " right ", " man " };

            // When the player has already played russian roulette with the man
            if (player.items.Contains("phone number"))
            {
                Console.WriteLine("-VIP LOUNGE-\n" +
                                  "You begin to make your way towards the middle of the VIP Lounge\n" +
                                  "On your left, you notice a sea of people cheering.\n" +
                                  "On your right however, there seems to be some sort of dead man.");
            }
            // When the player has not played russian roulette yet
            else
            {
                Console.WriteLine("-VIP LOUNGE-\n" +
                                  "You begin to make your way towards the middle of the VIP Lounge\n" +
                                  "On your left, you notice a sea of people cheering.\n" +
                                  "On your right however, there seems to be a man sitting alone, staring directly at you.");
            }

            UserInput choice = Program.InputCheck(player, left, go, right, go, room: true);

            if (choice == UserInput.ContainsFirstWords)
            {
                SnailRace.StartUp(player);
            }
            else
            {
                RussianRoulette.StartUp(player);
            }
        }
    }



    //---------------GAMES----------------

    //----------MAIN-CASINO-GAMES----------

    static class Blackjack
    {
        public static void StartUp(Player player)
        {
            Console.WriteLine("-BLACKJACK-\n" +
                              "You go over to the blackjack tables and the dealer asks you if you would like to play.\n" +
                              "Will you play?");
            UserInput play = Program.InputCheck(player, question: true);

            if (play == UserInput.Confirm)
            {
                Console.WriteLine("The dealer explains that the rules for blackjack at this casino are very simple:\n" +
                                  "Each turn you are asked whether or not you would like to hit (get a new card).\n" +
                                  "You can hit as many times as you want until you either go bust (go over 21) or choose not to hit (stand).\n");
            }

            // Blackjack game loop
            while (play == UserInput.Confirm)
            {
                Betting(player);

                Console.WriteLine("Would you like to play again?");
                play = Program.InputCheck(player, room: true, question: true);
            }

            Console.WriteLine("You chicken out and choose not to play blackjack for now.\n");
            Casino.Inside(player);
        }

        private static void Betting(Player player)
        {
            Console.WriteLine("How much would you like to bet?");
            int bet = Program.Bet(player);

            // Dealing with invalid bets
            if (int.Parse(player.money) < bet)
            {
                Console.WriteLine($"You search your pockets for the £{bet} you were going to bet but can't seem to find it anywhere.\n" +
                                   "Reluctantly you turn back towards the centre of the casino, hanging your head in shame.\n" +
                                   "Maybe next time you should come back with the funds!\n");
                Thread.Sleep(4000);
                Casino.Inside(player);
            }

            // Removing the money the user has betted from their inventory
            player.money = (int.Parse(player.money) - bet).ToString();

            GamePlay(player, bet);
        }

        private static void GamePlay(Player player, int bet)
        {
            int userValue, dealerValue;
            string number, card, suit;
            UserInput hit;
            string[] numbers = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            string[] suits = new string[] { "♥", "♦", "♠", "♣" };

            bool setup = true;
            bool end = false;
            List<string> cards = new List<string>();
            List<string> userDeck = new List<string>();
            List<string> dealerDeck = new List<string>();

            Random rnd = new Random();

            // Card shuffle
            for (int i = 0; i < 26; i++)
            {
                do
                {
                    number = numbers[rnd.Next(13)];
                    suit = suits[rnd.Next(4)];
                    card = $"{number} {suit}";
                } while (cards.Contains(card));
                cards.Add(card);
            }

            // Player hit loop
            do
            {
                userValue = 0;
                dealerValue = 0;

                // Inital dealing of two cards to the user and the dealer at the start of the game
                if (setup)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        userDeck.Add(cards[0]);
                        cards.RemoveAt(0);
                    }

                    dealerDeck.Add(cards[0]);
                    cards.RemoveAt(0);

                    setup = false;
                }
                else
                {
                    userDeck.Add(cards[0]);
                    cards.RemoveAt(0);
                }

                foreach (string c in userDeck)
                {
                    try // Happens when the card is a number card
                    {
                        userValue += int.Parse(c.Split(' ')[0]);
                    }
                    catch
                    {
                        if (c[0] != 'A') // Happens for all picture cards
                        {
                            userValue += 10;
                        }
                    }
                }

                // For all aces in the deck, sets the value of the ace to the number that gets it closest to 21 without going over
                // Allows for the ace to equal either 1 or 11 throughout the course of the game
                foreach (string c in userDeck)
                {
                    if (c[0] == 'A')
                    {
                        if (userValue + 11 > 21)
                        {
                            userValue += 1;
                        }
                        else
                        {
                            userValue += 11;
                        }
                    }
                }

                // Finding the value of the dealer's first card
                try
                {
                    dealerValue += int.Parse(dealerDeck[0].Split(' ')[0]);
                }
                catch
                {
                    if (dealerDeck[0][0] == 'A')
                    {
                        dealerValue += 11;
                    }
                    else
                    {
                        dealerValue += 10;
                    }
                }

                if (userValue >= 21) // Immediately ends the game once the user reaches 21 or above
                {
                    DisplayDeck(userValue, dealerValue, userDeck, dealerDeck);
                    end = true;
                }

                // Allows the user to hit if they are not yet at or above 21
                hit = UserInput.Disconfirm;
                if (!end)
                {
                    DisplayDeck(userValue, dealerValue, userDeck, dealerDeck);
                    Console.WriteLine("Would you like to hit?");
                    hit = Program.InputCheck(player, question: true, inputLock: true);
                }
            } while (hit == UserInput.Confirm);

            // Allows the dealers turn to playout, given that the user has not already won or lost
            if (!end)
            {
                do
                {
                    dealerValue = 0;
                    dealerDeck.Add(cards[0]);
                    cards.RemoveAt(0);

                    foreach (string c in dealerDeck)
                    {
                        try
                        {
                            dealerValue += int.Parse(c.Split(' ')[0]);
                        }
                        catch
                        {
                            if (c[0] != 'A')
                            {
                                dealerValue += 10;
                            }
                        }
                    }

                    foreach (string c in dealerDeck)
                    {
                        if (c[0] == 'A')
                        {
                            if (dealerValue + 11 > 21)
                            {
                                dealerValue += 1;
                            }
                            else
                            {
                                dealerValue += 11;
                            }
                        }
                    }
                } while (dealerValue < 17); // Stops dealer's turns once they are at or above 17

                //-----DISPLAYING-USER'S-CARDS-----
                Console.WriteLine($"Your Hand (value = {userValue}):");

                // Top of each card
                foreach (string c in userDeck)
                {
                    Console.Write("┌─┐ ");
                }
                Console.WriteLine();

                // Value of each card
                foreach (string c in userDeck)
                {
                    if (c.Split(' ')[0] == "10")
                        Console.Write($"│{c.Split(' ')[0]} ");
                    else
                        Console.Write($"│{c.Split(' ')[0]}│ ");
                }
                Console.WriteLine();

                // Suit of each card
                foreach (string c in userDeck)
                {
                    Console.Write($"│{c.Split(' ')[1]}│ ");
                }
                Console.WriteLine();

                // Bottom of each card
                foreach (string c in userDeck)
                {
                    Console.Write("└─┘ ");
                }

                //-----DISPLAYING-DEALER'S-CARDS-----
                Console.WriteLine($"\n\nDealer's Hand (value = {dealerValue}):");

                // Top of each card
                foreach (string c in dealerDeck)
                {
                    Console.Write("┌─┐ ");
                }
                Console.WriteLine();

                // Value of each card
                foreach (string c in dealerDeck)
                {
                    if (c.Split(' ')[0] == "10")
                        Console.Write($"│{c.Split(' ')[0]} ");
                    else
                        Console.Write($"│{c.Split(' ')[0]}│ ");
                }
                Console.WriteLine();

                // Suit of each card
                foreach (string c in dealerDeck)
                {
                    Console.Write($"│{c.Split(' ')[1]}│ ");
                }
                Console.WriteLine();

                // Bottom of each card
                foreach (string c in dealerDeck)
                {
                    Console.Write("└─┘ ");
                }
                Console.WriteLine("\n");
            }

            Payout(player, bet, userValue, dealerValue);
        }

        private static void DisplayDeck(int userValue, int dealerValue, List<string> userDeck, List<string> dealerDeck)
        {
            // Method is only used for when the dealer's turns don't have to be played out for the end of the game
            // This is since this method always displays the second card the dealer has as a blank (not yet turned over)

            //-----DISPLAYING-USER'S-CARDS-----
            Console.WriteLine($"Your Hand (value = {userValue}):");

            // Top of each card
            foreach (string card in userDeck)
            {
                Console.Write("┌─┐ ");
            }
            Console.WriteLine();

            // Value of each card
            foreach (string card in userDeck)
            {
                if (card.Split(' ')[0] == "10")
                    Console.Write($"│{card.Split(' ')[0]} ");
                else
                    Console.Write($"│{card.Split(' ')[0]}│ ");
            }
            Console.WriteLine();

            // Suit of each card
            foreach (string card in userDeck)
            {
                Console.Write($"│{card.Split(' ')[1]}│ ");
            }
            Console.WriteLine();

            // Bottom of each card
            foreach (string card in userDeck)
            {
                Console.Write("└─┘ ");
            }

            //-----DISPLAYING-DEALER'S-CARDS-----
            Console.WriteLine($"\n \nDealer's Hand (value = {dealerValue}):");

            Console.WriteLine("┌─┐ ┌─┐");
            if (dealerDeck[0].Split(' ')[0] == "10")
                Console.WriteLine($"│{dealerDeck[0].Split(' ')[0]} │ │");
            else
                Console.WriteLine($"│{dealerDeck[0].Split(' ')[0]}│ │ │");
            Console.WriteLine($"│{dealerDeck[0].Split(' ')[1]}│ │ │");
            Console.WriteLine("└─┘ └─┘\n");
        }

        private static void Payout(Player player, int bet, int userValue, int dealerValue)
        {
            if (userValue == 21 || (userValue > dealerValue && userValue < 21) || dealerValue > 21 && userValue < 21)
            {
                bet *= 2;
                Console.WriteLine($"Congratulations! You won £{bet}\n");
            }
            else if (userValue == dealerValue || (userValue > 21 && dealerValue > 21))
            {
                Console.WriteLine("It was a draw. Money back.\n");
            }
            else
            {
                Console.WriteLine($"You lost £{bet} :(\n");
                bet = 0;
            }

            // Adding the money won by the user to their inventory (if they have lost this is 0)
            player.money = (int.Parse(player.money) + bet).ToString();
        }
    }

    static class Roulette
    {
        public static void StartUp(Player player)
        {
            Console.WriteLine("-ROULETTE-\n" +
                              "You go over to the roulette table and one of the staff asks you if you would like to place a bet.\n" +
                              "Will you play?");
            UserInput play = Program.InputCheck(player, room: true, question: true);

            if (play == UserInput.Confirm)
            {
                Console.WriteLine("First, the staff notifies you of the possible bets you can make, along with their payout multipliers:\n" +
                                  "[x36] Single number\n" +
                                  "[x 3] Dozens (1-12, 13-24, 25-36)\n" +
                                  "[x 3] Columns (1st, 2nd, 3rd)\n" +
                                  "[x 2] Halves (1-18, 19-36)\n" +
                                  "[x 2] Colours (red, black)\n");
            }

            // Roulette game loop
            while (play == UserInput.Confirm)
            {
                Betting(player);

                Console.WriteLine("Would you like to play again?");
                play = Program.InputCheck(player, room: true, question: true);
            }

            Console.WriteLine("You decide not to partake in roulette this time.\n");
            Casino.Inside(player);
        }

        private static void Betting(Player player)
        {
            Console.WriteLine("How much would you like to bet?");
            int bet = Program.Bet(player);

            // Dealing with invalid bets
            if (int.Parse(player.money) < bet)
            {
                Console.WriteLine($"You search your pockets for the £{bet} you were going to bet but can't seem to find it anywhere.\n" +
                                   "Reluctantly you turn back towards the centre of the casino, hanging your head in shame.\n" +
                                   "Maybe next time you should come back with the funds!\n");
                Thread.Sleep(4000);
                Casino.Inside(player);
            }

            // Removing the money the user has betted from their inventory
            player.money = (int.Parse(player.money) - bet).ToString();

            bool first = true;
            string bettingSpace;
            string[] rouletteBets = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18",
                                                   "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "32", "33", "34", "35", "36",
                                                   "1-12", "13-24", "25-36", "1st", "first", "2nd", "second", "3rd", "third", "1-18", "19-36", "red", "black"};

            Console.WriteLine("What would you like to place your bet on?");

            // User input loop until they enter an accepted betting space
            do
            {
                if (!first)
                {
                    Console.WriteLine("\nInvalid input!\n" +
                                      "Please make sure you enter a recognised betting space, e.g. \"7\", \"25-36\", \"2nd\", \"1-18\", or \"red\"");
                }

                Console.Write(">");
                bettingSpace = Console.ReadLine().Trim(' ').ToLower();
                first = false;
            } while (!Array.Exists(rouletteBets, x => bettingSpace == x));

            GamePlay(player, bet, bettingSpace);
        }

        private static void GamePlay(Player player, int bet, string bettingSpace)
        {
            string result;

            // Generating the number the ball lands on
            Random rnd = new Random();
            int number = rnd.Next(1, 37);

            // All evens are red
            if (number % 2 == 0)
            {
                result = $"red {number}";
            }
            // All odds are black
            else
            {
                result = $"black {number}";
            }

            Payout(player, bet, bettingSpace, result, number);
        }

        private static void Payout(Player player, int bet, string bettingSpace, string result, int number)
        {
            Console.WriteLine($"\nThe ball landed on: {result}");

            int multiplier = 0;
            switch (bettingSpace) // Switch statement for all possible betting spaces
            {
                case "1-12":
                    if (number < 13)
                        multiplier = 3;
                    break;

                case "13-24":
                    if (number > 12 && number < 25)
                        multiplier = 3;
                    break;

                case "25-36":
                    if (number > 24)
                        multiplier = 3;
                    break;

                case "1st":
                case "first":
                    if (number % 3 == 1)
                        multiplier = 3;
                    break;

                case "2nd":
                case "second":
                    if (number % 3 == 2)
                        multiplier = 3;
                    break;

                case "3rd":
                case "third":
                    if (number % 3 == 0)
                        multiplier = 3;
                    break;

                case "1-18":
                    if (number < 19)
                        multiplier = 2;
                    break;

                case "19-36":
                    if (number > 18)
                        multiplier = 2;
                    break;

                case "red":
                    if (result.StartsWith("red"))
                        multiplier = 2;
                    break;

                case "black":
                    if (result.StartsWith("black"))
                        multiplier = 2;
                    break;

                default: // For any single numbers betted on
                    if (number == int.Parse(bettingSpace))
                        multiplier = 36;
                    break;
            }

            if (multiplier != 0)
            {
                Console.WriteLine($"Congratulations! You won £{bet * multiplier}\n");
            }
            else
            {
                Console.WriteLine($"You lost £{bet} :(\n");
            }

            bet *= multiplier;

            // Adding the money won by the user to their inventory (if they have lost this is 0)
            player.money = (int.Parse(player.money) + bet).ToString();
        }
    }
    static class Slots
    {
        public static void StartUp(Player player)
        {
            Console.WriteLine("-SLOTS-\n" +
                              "You head over to the slot machines and see one that gives you really good vibes.\n" +
                              "Do you test your luck at slots?");
            UserInput play = Program.InputCheck(player, room: true, question: true);

            if (play == UserInput.Confirm)
            {
                Console.WriteLine("This slot machine allows you to input as much money as you want.\n" +
                                  "The payout multipliers are as follows:\n" +
                                  "[x 2] OOO (three Os in a row)\n" +
                                  "[x 5] XXX (three Xs a row)\n" +
                                  "[x15] $$$ (three $s in a row)\n");
            }

            // Slots game loop
            while (play == UserInput.Confirm)
            {
                Betting(player);

                Console.WriteLine("Would you like to play again?");
                play = Program.InputCheck(player, room: true, question: true);
            }

            Console.WriteLine("You decide against playing slots for the time being.\n");
            Casino.Inside(player);
        }

        private static void Betting(Player player)
        {
            Console.WriteLine("How much would you like to bet?");
            int bet = Program.Bet(player);

            // Dealing with invalid bets
            if (int.Parse(player.money) < bet)
            {
                Console.WriteLine($"You search your pockets for the £{bet} you were going to bet but can't seem to find it anywhere.\n" +
                                   "Reluctantly you turn back towards the centre of the casino, hanging your head in shame.\n" +
                                   "Maybe next time you should come back with the funds!\n");
                Thread.Sleep(4000);
                Casino.Inside(player);
            }

            // Removing the money the user has betted from their inventory
            player.money = (int.Parse(player.money) - bet).ToString();

            GamePlay(player, bet);
        }

        private static void GamePlay(Player player, int bet)
        {
            string[] symbols = new string[] { "O", "X", "$" };
            Random rnd = new Random();

            // Generating and displaying slot result
            string result = $"{symbols[rnd.Next(3)]}{symbols[rnd.Next(3)]}{symbols[rnd.Next(3)]}";
            Console.WriteLine($"{symbols[rnd.Next(3)]}|{symbols[rnd.Next(3)]}|{symbols[rnd.Next(3)]}\n" +
                              $"{result[0]}|{result[1]}|{result[2]} <--\n" +
                              $"{symbols[rnd.Next(3)]}|{symbols[rnd.Next(3)]}|{symbols[rnd.Next(3)]}");

            Payout(player, bet, result);
        }

        private static void Payout(Player player, int bet, string result)
        {
            int multiplier = 0;
            switch (result) // Switch statement for possible winning combinations
            {
                case "OOO":
                    multiplier = 2;
                    break;

                case "XXX":
                    multiplier = 5;
                    break;

                case "$$$":
                    multiplier = 15;
                    Console.Write("You hit the JACKPOT! ");
                    break;

                default: // If the player has not won, multiplier remains as 0
                    break;
            }

            if (multiplier != 0)
            {
                Console.WriteLine($"Congratulations! You won £{bet * multiplier}\n");
            }
            else
            {
                Console.WriteLine($"You lost £{bet} :(\n");
            }

            bet *= multiplier;

            // Adding the money won by the user to their inventory (if they have lost this is 0)
            player.money = (int.Parse(player.money) + bet).ToString();
        }
    }



    //----------VIP-LOUNGE-GAMES----------
    static class SnailRace
    {
        public static void StartUp(Player player)
        {
            Console.WriteLine("You walk towards the crowd to see that they are huddled around some sort of mini arena.\n" +
                             "As you get closer you realise that these people are betting on snail races.\n" +
                             "Suddenly, you get asked if you have a snail you would like to race.\n" +
                             "Will you race your snail?");
            UserInput play = Program.InputCheck(player, room: true, question: true);

            if (player.items.Contains("snail"))
            {
                Console.WriteLine("You prepare your snail while someone tells you the payout multiplier for winning is x3.");
            }

            while (play == UserInput.Confirm)
            {
                // Checks to make sure user still has snail
                if (!player.items.Contains("snail"))
                {
                    Console.WriteLine("You decide that you're going to race your snai-\n" +
                                      "Wait a second, you don't have a snail to race with!\n" +
                                      "Everyone laughs at you as you walk away, disappointed.\n");
                    Thread.Sleep(3000);
                    VIPlounge.Inside(player);
                }

                Betting(player);

                Thread.Sleep(3000);
                Console.WriteLine("Would you like to race again?");
                play = Program.InputCheck(player, room: true, question: true);
            }

            Console.WriteLine("You decide not to test your luck at the races for now.\n");
            VIPlounge.Inside(player);
        }

        private static void Betting(Player player)
        {
            Console.WriteLine("How much would you like to bet?");
            int bet = Program.Bet(player);

            // Dealing with invalid bets
            if (int.Parse(player.money) < bet)
            {
                Console.WriteLine($"You search your pockets for the £{bet} you were going to bet but can't seem to find it anywhere.\n" +
                                   "Reluctantly you turn back towards the centre of the VIP Lounge, hanging your head in shame.\n" +
                                   "Maybe next time you should come back with the funds!\n");
                Thread.Sleep(4000);
                VIPlounge.Inside(player);
            }

            // Removing the money the user has betted from their inventory
            player.money = (int.Parse(player.money) - bet).ToString();

            GamePlay(player, bet);
        }

        private static void GamePlay(Player player, int bet)
        {
            Random rnd = new Random();
            int result = rnd.Next(4);

            int multiplier = 0;
            switch (result) // Switch statement for all possible race outcomes
            {
                case 0:
                    multiplier = 3;
                    Console.WriteLine("Your snail starts off with an early lead.\n" +
                                      "No other snail can match its speed.\n" +
                                      "It easily crosses the finish line with time to spare.");

                    if (rnd.Next(3) != 0) // Creates 2 in 3 chance that you are accused of cheating when snail wins convincingly
                    {
                        Console.WriteLine($"Congratulations! You won £{bet * 3}\n");
                        Thread.Sleep(3000);
                        Console.WriteLine("But before you can take your winnings, someone starts saying you're a cheater.\n" +
                                          "Now everyone is accusing you of using an enhanced snail!\n" +
                                          "A black bag gets pulled over your head and your hands are tied before being dragged to another room.\n");
                        Thread.Sleep(5000);
                        StaffOnly.Inside(player);
                    }

                    break;
                case 1:
                    multiplier = 3;
                    Console.WriteLine("It seems like there is no hope for your snail.\n" +
                                      "After a rough start it looks like its going to end in last place.\n" +
                                      "But wait a second, it's getting closer, and closer... it's taken the lead!\n" +
                                      "And it crosses the finish line in a split-second finish.");
                    break;
                case 2:
                    Console.WriteLine("It looks like your snail has this in the bag.\n" +
                                      "But wait, it seems to have slowed down just a few centimetres from the finish line.\n" +
                                      "And it gives away the win to the another snail!");
                    break;
                case 3:
                    Console.WriteLine("Before the race even begins, your snail collapses from anxiety.\n" +
                                      "It doesn't look like it can ever race again.");
                    player.RemoveItem("snail"); // Removes snail from user inventory when it is unable to race
                    break;
            }

            Payout(player, bet, multiplier, result);
        }

        private static void Payout(Player player, int bet, int multiplier, int result)
        {
            if (result <= 1)
            {
                Console.WriteLine($"Congratulations! You won £{bet * 3}\n");
            }
            else
            {
                Console.WriteLine($"You lost £{bet} :(\n");
            }

            bet *= multiplier;

            // Adding the money won by the user to their inventory (if they have lost this is 0)
            player.money = (int.Parse(player.money) + bet).ToString();
        }
    }

    static class RussianRoulette
    {
        public static void StartUp(Player player)
        {
            // Occurs when player has already played and won russian roulette
            if (player.items.Contains("phone number"))
            {
                Console.WriteLine("You walk over to the corpse but there isn't really anything else in this area.\n" +
                                  "You then decide to head back to the centre of the VIP lounge.\n");
                Thread.Sleep(3000);
                VIPlounge.Inside(player);
            }

            Console.WriteLine("You walk towards the man, scared but also intrigued.\n" +
                              "You ask him why he keeps looking at you.\n" +
                              "He says he wants to know if you would like to play some russian roulette with him.\n" +
                              "Will you play? Note that once you have accepted, you cannot back down.");

            UserInput play = Program.InputCheck(player, question: true, room: true);

            if (play == UserInput.Disconfirm)
            {
                Console.WriteLine("Despite the potential thrill of it, you decide you don't want to risk your life today!\n" +
                                  "You then head back towards the centre of the VIP lounge.\n");
                Thread.Sleep(3000);
                VIPlounge.Inside(player);
            }
            else if (play == UserInput.Confirm)
            {
                Console.WriteLine("You decide to participate!\n" +
                                  "The man explains that he has a gun which takes 6 bullets.\n" +
                                  "Five of the slots are empty but one is loaded.\n" +
                                  "You will each take turns aiming the gun at yourselves before pulling the trigger.\n" +
                                  "First one to die loses!\n" +
                                  "The man says that he will go first.\n");

                Thread.Sleep(7000);

                Random rnd = new Random();
                int bullet = rnd.Next(6); // Generates bullets position

                for (int i = 0; i < bullet; i++) // Loop while bullet position has not yet been reached
                {
                    if (i % 2 == 0)
                    {
                        Console.WriteLine("The man aims the gun at his head and pulls the trigger.\n" +
                                          "Nothing!\n");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("You aim the gun at your head and pull the trigger.\n" +
                                          "Nothing!\n");
                        Thread.Sleep(2000);
                    }
                }

                // If bullet position is even, the man dies
                if (bullet % 2 == 0)
                {
                    Console.WriteLine("The man aims the gun at his head and pulls the trigger.\n" +
                                      "He shoots himself in the head and dies!\n");
                    Thread.Sleep(2000);
                    Console.WriteLine("You go over to his body and notice a piece of paper coming out of his pocket\n" +
                                      $"It is addressed to {player.name}, followed by a phone number, weird...\n" +
                                      "You then head back towards the middle of the VIP lounge\n");
                    Thread.Sleep(4000);
                    player.AddItem("phone number");
                    VIPlounge.Inside(player);
                }
                // If bullet position is odd, user dies
                else
                {
                    Console.WriteLine("You aim the gun at your head and pull the trigger.\n" +
                                      "You shoot yourself in the head and die!\n");
                    Thread.Sleep(2000);
                    Console.WriteLine("GAME OVER.");
                    player.EraseData();
                    Environment.Exit(0);
                }
            }
        }
    }



    //---------------PLAYER-CLASS---------------

    sealed class Player
    {
        public string name, money, items, password, time, playerData;

        // Constructor for players with saved data
        public Player(string playerName, string playerMoney, string playerItems, string playerPassword, string playerTime)
        {
            name = playerName;
            money = playerMoney;
            items = playerItems;
            password = playerPassword;
            time = playerTime;
        }

        // Constructor for new players
        public Player(string playerName, string playerPassword)
        {
            name = playerName;
            money = "30";
            items = "";
            password = playerPassword;
            time = "0m0s";

            UpdateData();
        }

        // Static class to check whether a player has saved data or is new, before creating an instance of the Player class
        public static bool UniqueNameCheck(string name)
        {
            string response;
            bool repeat = false;

            StreamReader sr = new StreamReader(Program.GAME_DATA_FILE);

            // Looping through game data file
            while (!sr.EndOfStream)
            {
                string randomData = sr.ReadLine();

                if (name == randomData.Split(';')[0].Split(':')[1]) // Compares name enter by player to the name of each user in the file
                {
                    sr.Close();

                    Console.WriteLine("\nA player with this name already exists -");
                    DisplayData(name, randomData.Split(';')[1].Split(':')[1], randomData.Split(';')[2].Split(':')[1]);

                    // Loop until user enters a valid response
                    do
                    {
                        if (repeat)
                        {
                            Console.WriteLine("Invalid Input!\n" +
                                              "Please reply with \"yes\" or \"no\"\n");
                        }

                        Console.Write("Is this you? ");
                        response = Console.ReadLine();

                        repeat = true;
                    } while (response != "yes" && response != "no");

                    // If the two users are not the same, the user is returned to the start of the program to enter a unique username
                    if (response == "no")
                    {
                        Console.WriteLine();
                        Program.Main();
                    }
                    // If the player claims that they are the person saved in the file, they are prompted to enter the password for this user
                    else
                    {
                        Console.Write("Please enter your password: ");
                        string guess = Console.ReadLine();

                        // If the player enters the incorrect password for the saved user, the game is quit
                        if (guess != randomData.Split(';')[3].Split(':')[1])
                        {
                            Console.WriteLine("Incorrect password entered! Quitting game...");
                            Environment.Exit(0);
                        }
                        Console.WriteLine();

                        // If the player enters the correct password the variables are updated in the program class to allow an instance of the player
                        // with the saved data of this user to be created, allowing the player to continue from where they left off
                        Program.savedMoney = randomData.Split(';')[1].Split(':')[1];
                        Program.savedItems = randomData.Split(';')[2].Split(':')[1];
                        Program.savedPassword = guess;
                        Program.savedTime = randomData.Split(';')[4].Split(':')[1];
                        return false;
                    }
                }
            }

            sr.Close();
            return true;
        }

        // Writes all data stored in the fields for the player object to the game data file
        public void UpdateData()
        {
            string randomData;
            bool found = false;

            playerData = $"Name:{name};Money:{money};Items:{items};Password:{password};Time:{time}";
            StreamReader sr = new StreamReader(Program.GAME_DATA_FILE);
            List<string> allData = new List<string>();

            // Loops through game data file
            while (!sr.EndOfStream)
            {
                randomData = sr.ReadLine();

                // Replaces data in file for the user with the current data stored in the player object
                if (randomData.Split(';')[0].Split(':')[1] == name)
                {
                    found = true;
                    allData.Add(playerData);
                }
                else
                {
                    allData.Add(randomData);
                }
            }
            sr.Close();

            // Writes the save data for a new user to the file
            if (!found)
            {
                StreamWriter sw = new StreamWriter(Program.GAME_DATA_FILE, append: true);
                sw.WriteLine(playerData);
                sw.Close();
            }
            // Updates the data for a user with saved data
            else
            {
                StreamWriter sw = new StreamWriter(Program.GAME_DATA_FILE);
                foreach (string data in allData)
                {
                    sw.WriteLine(data);
                }
                sw.Close();
            }
        }

        // Delete all data saved for an account, without deleting the account itself (i.e. the name and password)
        public void EraseData()
        {
            StreamReader sr = new StreamReader(Program.GAME_DATA_FILE);
            List<string> allUserData = new List<string>();

            // Loops through game data file
            while (!sr.EndOfStream)
            {
                string randomData = sr.ReadLine();
                string[] randomDataSplit = randomData.Split(';');
                string randomName = randomDataSplit[0].Split(':')[1];

                if (name != randomName)
                {
                    allUserData.Add(randomData);
                }
                // Resets the data for user
                else
                {
                    money = "30";
                    items = "";
                    time = "0m0s";

                    allUserData.Add($"Name:{name};Money:30;Items:;Password:{password};Time:0m0s");
                }
            }
            sr.Close();

            // Updates the game data file accordingly
            StreamWriter sw = new StreamWriter(Program.GAME_DATA_FILE);
            foreach (string userData in allUserData)
            {
                sw.WriteLine(userData);
            }
            sw.Close();
        }

        // Deletes the account of a player
        public void DeletePlayer()
        {
            StreamReader sr = new StreamReader(Program.GAME_DATA_FILE);
            List<string> allUserData = new List<string>();

            // Loops through game data file
            while (!sr.EndOfStream)
            {
                string randomData = sr.ReadLine();
                string[] randomDataSplit = randomData.Split(';');
                string randomName = randomDataSplit[0].Split(':')[1];

                if (name != randomName)
                {
                    allUserData.Add(randomData);
                }
            }
            sr.Close();

            // Updates the game data file to have every account other than the current users
            StreamWriter sw = new StreamWriter(Program.GAME_DATA_FILE);
            foreach (string userData in allUserData)
            {
                sw.WriteLine(userData);
            }
            sw.Close();
        }

        // Displays the current data stored in the player object to the console
        public void DisplayData()
        {
            UpdateData();

            string itemsDisplay = items == "" ? "N/A" : items.Replace(",", ", ");
            Console.WriteLine($"──USER-DATA──\n" +
                              $"Name: {name}\n" +
                              $"Money: £{money}\n" +
                              $"Items: {itemsDisplay}\n");
        }

        // Overload of other DisplayData method to display the data for a user before creating an instance of the player class
        // This is to allow the program to display the data for a user in the UniqueNameCheck, so that they can verify whether or not it is them
        public static void DisplayData(string name, string money, string items)
        {
            string itemsDisplay = items == "" ? "N/A" : items.Replace(",", ", ");
            Console.WriteLine($"──USER-DATA──\n" +
                              $"Name: {name}\n" +
                              $"Money: £{money}\n" +
                              $"Items: {itemsDisplay}\n");
        }

        public void AddItem(string item)
        {
            UpdateData();

            if (items == "")
            {
                items = item;
            }
            else
            {
                items += $",{item}";
            }

            UpdateData();
        }

        public void RemoveItem(string item)
        {
            UpdateData();

            string newItems = "";

            foreach (string i in items.Split(','))
            {
                if (i != item)
                {
                    newItems += $",{i}";
                }
            }

            items = newItems.TrimStart(',');
            UpdateData();
        }
    }



    //---------------MAIN-PROGRAM---------------

    class Program
    {
        public static string savedMoney, savedItems, savedPassword, savedTime;
        public static Stopwatch timer = new Stopwatch();
        public static string GAME_DATA_FILE = @"/Users/jamesharrison/Desktop/Coding/C#/TextBasedGame/TextBasedGame/GameData.txt";
        public static string MAP = "             ___─────CASINO-MAP─────___            \n" +
                                   "┌───────────────────┬─────────────────────────────┐\n" +
                                   "│                   │                             │\n" +
                                   "│                   │          VIP Lounge         │\n" +
                                   "│   Bar             │                             │\n" +
                                   "│        ┌──── ─────┴────────────── ────┬─────────┤\n" +
                                   "│        │                              │         │\n" +
                                   "│        │                              │   Staff │\n" +
                                   "│        │                              │   Only  │\n" +
                                   "│        │    Blackjack       Slot      │         │\n" +
                                   "│        │     Tables       Machines    │         │\n" +
                                   "│        │                              └─────────┤\n" +
                                   "├────────┤                                        │\n" +
                                   "│        │                                        │\n" +
                                   "│        │                                        │\n" +
                                   "│  Shop  │                             Roulette   │\n" +
                                   "│                                       Table     │\n" +
                                   "│        │                                        │\n" +
                                   "│        │                                        │\n" +
                                   "└────────┴────────────────Entrance────────────────┘\n";

        public static void Main()
        {
            Player player;

            Console.WriteLine("---------------CASINO-ESCAPE---------------");

            Console.Write("Please enter your name: ");
            string name = Console.ReadLine();

            // Creates a new player
            if (Player.UniqueNameCheck(name))
            {
                Console.Write("Please enter a password that you can remember: ");
                string password = Console.ReadLine();
                Console.WriteLine();

                player = new Player(name, password);
            }
            // Creates a player with saved data
            else
            {
                player = new Player(name, savedMoney, savedItems, savedPassword, savedTime);
            }

            Casino.StartUp(player);
        }

        public static int Bet(Player player)
        {
            string[] inventoryNouns = new string[] { " inventory ", " savings ", " balance ", " name ", " money ", " items ", " data ", " inv " };
            string[] displayVerbs = new string[] { " open ", " check ", " display ", " view ", " look at ", " use " };

            // Loops until valid bet is entered
            while (true)
            {
                Console.Write(">");
                string action = $" {Console.ReadLine()} ";
                Console.WriteLine();

                if (Array.Exists(displayVerbs, x => action.Contains(x))) // Allows the user to check their savings before betting
                {
                    player.DisplayData();
                }
                else
                {
                    action = action.ToLower().Trim().TrimStart('£');

                    try
                    {
                        // Check to make sure bet is a positive integer
                        int bettingAmount = int.Parse(action);

                        if (bettingAmount > 0)
                        {
                            return bettingAmount;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        // Error message for invalid bets
                        Console.WriteLine("Invalid input!\n" +
                                          "If you are asked for an amount to bet, please reply with a positive integer, e.g. \"5\" or \"100\".");
                    }
                }
            }
        }

        public static UserInput InputCheck(Player player,
                                           string[] nouns1 = null, string[] verbs1 = null,
                                           string[] nouns2 = null, string[] verbs2 = null,
                                           string[] nouns3 = null, string[] verbs3 = null,
                                           bool question = false,
                                           bool room = false,
                                           bool inputLock = false)
        {
            string[] inventoryNouns, displayVerbs, negatives, confirm, disconfirm, enter, items, games, rooms, call, delete, restart, quit, account;

            inventoryNouns = new string[] { " inventory ", " savings ", " balance ", " name ", " money ", " items ", " data ", " inv " };
            displayVerbs = new string[] { " open ", " check ", " display ", " view ", " look at ", " use " };
            negatives = new string[] { " not ", " don't ", " dont " };
            confirm = new string[] { "confirm", "yes", "ok", "okay", "agree", "sure" };
            disconfirm = new string[] { "deny", "no", "nope", "disagree" };
            enter = new string[] { " go in ", " enter ", " go to ", " go inside " };
            items = new string[] { " map ", " baseball bat ", " telephone " };
            games = new string[] { " roulette ", " slots ", " slot ", " blackjack " };
            rooms = new string[] { " bar ", " vip lounge ", " shop ", " staff only " };
            call = new string[] { " call ", " ring ", " phone ", " telephone " };
            delete = new string[] { " delete ", " remove ", " erase " };
            restart = new string[] { " restart ", " reset " };
            quit = new string[] { " quit ", " leave " };
            account = new string[] { " account ", " game " };

            bool repeat, invalid;

            do
            {
                repeat = true;
                invalid = false;

                Console.Write(">");
                string action = $" {Console.ReadLine().ToLower().TrimStart('£')} ";
                Console.WriteLine();

                // Rejects any inputs containing negatives
                if (Array.Exists(negatives, x => action.Contains(x)))
                {
                    Console.WriteLine("Invalid input!\n" +
                                      "Please make sure your input does not use negatives unless specifically asked a question where you must reply no.");
                }
                // Ends game when user calls phone number
                else if (Array.Exists(call, x => action.Contains(x)) && action.Contains(" number ") && player.items.Contains("telephone") && player.items.Contains("telephone") && !inputLock)
                {
                    Console.WriteLine("You begin to call the phone number that you got from that man in the VIP lounge...\n" +
                                      "You wake up laying in your bed to the noise of your phone ringing and realise you were dreaming all this time.\n" +
                                      "Congratulations! You have won the game!\n");
                    Thread.Sleep(4000);
                    Console.WriteLine("THE END!");

                    // Obtains elapsed time
                    TimeSpan ts = timer.Elapsed;
                    timer.Stop();

                    // Adds elapsed time during current session to any saved elapsed time in the player's saved data
                    int m = int.Parse(player.time.Split('m')[0]) + ts.Minutes;
                    int s = int.Parse(player.time.Split('m')[1].TrimEnd('s')) + ts.Seconds;

                    // Converts any blocks of 60 seconds to minutes
                    if (s >= 60)
                    {
                        s -= 60;
                        m += 1;
                    }

                    // Checking whether seconds and minutes are plural or not and updating them accordingly
                    string minutes = m == 1 ? "MINUTE" : "MINUTES";
                    string seconds = s == 1 ? "SECOND" : "SECONDS";

                    // Displaying elapsed time to the player
                    string elapsedTime = $"{m} {minutes} & {s} {seconds}";
                    Console.WriteLine("TIME TAKEN: " + elapsedTime);

                    player.EraseData();
                    Environment.Exit(0);
                }
                // Checking for inputs relating to user wanting to delete their account
                else if ((Array.Exists(delete, x => action == x) && !inputLock) || (Array.Exists(delete, x => action.Contains(x)) && Array.Exists(account, x => action.Contains(x)) && !inputLock))
                {
                    Console.WriteLine("Are you sure you would like to delete your account?\n" +
                                      "You will have to set up a new username and password the next time you play the game.");
                    UserInput choice = InputCheck(player, question: true, inputLock: true);

                    if (choice == UserInput.Confirm)
                    {
                        Console.WriteLine("Deleting account...");
                        timer.Reset();
                        player.DeletePlayer();
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Delete unsuccessful.\n");
                    }
                }
                // Checking for inputs relating to user wanting to restart their account
                else if ((Array.Exists(restart, x => action == x) && !inputLock) || (Array.Exists(restart, x => action.Contains(x)) && Array.Exists(account, x => action.Contains(x)) && !inputLock))
                {
                    Console.WriteLine("Are you sure you would like to restart?\n" +
                                      "All data linked to your name will be erased.");
                    UserInput choice = InputCheck(player, question: true, inputLock: true);

                    if (choice == UserInput.Confirm)
                    {
                        Console.WriteLine("Restarting game...\n");
                        timer.Reset();
                        player.EraseData();
                        Main();
                    }
                    else
                    {
                        Console.WriteLine("Restart unsuccessful.\n");
                    }
                }
                // Checking for inputs relating to user wanting to quit the game
                else if ((Array.Exists(quit, x => action == x) && !inputLock) || (Array.Exists(quit, x => action.Contains(x)) && Array.Exists(account, x => action.Contains(x)) && !inputLock))
                {
                    Console.WriteLine("Are you sure you would like to quit?\n" +
                                      "All data linked to your name will be saved until you return.");
                    UserInput choice = InputCheck(player, question: true, inputLock: true);

                    if (choice == UserInput.Confirm)
                    {
                        Console.WriteLine("Quitting game...");

                        // Obtains elapsed time
                        TimeSpan ts = timer.Elapsed;
                        timer.Stop();

                        // Adds elapsed time during current session to any saved elapsed time in the player's saved data
                        int m = int.Parse(player.time.Split('m')[0]) + ts.Minutes;
                        int s = int.Parse(player.time.Split('m')[1].TrimEnd('s')) + ts.Seconds;

                        // Converts any blocks of 60 seconds to minutes
                        if (s >= 60)
                        {
                            s -= 60;
                            m += 1;
                        }

                        // Updates saved total playthrough time for player so far
                        player.time = $"{m}m{s}s";
                        player.UpdateData();

                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Quit unsuccessful.\n");
                    }
                }
                // Checking for inputs relating to user wanting to use an item/inventory
                else if (Array.Exists(displayVerbs, x => action.Contains(x)) && !inputLock)
                {
                    if (Array.Exists(items, x => action.Contains(x)))
                    {
                        string foundItem = "";
                        bool found = false;

                        foreach (string item in items)
                        {
                            if (action.Contains(item) && !found)
                            {
                                foundItem = item.Trim();
                                found = true;
                            }
                        }

                        if (player.items.Contains("map") && foundItem == "map")
                        {
                            Console.WriteLine(MAP);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input!\n" +
                                              "You do not currently have a map of the casino.");
                        }
                    }
                    else if (Array.Exists(inventoryNouns, x => action.Contains(x)) && !inputLock)
                    {
                        player.DisplayData();
                    }
                    else
                    {
                        invalid = true;
                    }
                }
                // Checking for inputs relating to going to a room or playing a game
                else if (room && Array.Exists(enter, x => action.Contains(x)) && Array.Exists(rooms, x => action.Contains(x)) && !inputLock || room && Array.Exists(enter, x => action.Contains(x)) && Array.Exists(games, x => action.Contains(x)) && !inputLock || room && action.Contains("play") && !inputLock)
                {
                    // Checking for inputs relating to user wanting to go to a room
                    if (Array.Exists(rooms, x => action.Contains(x)) && !(action.Contains("play")))
                    {
                        if (action.Contains(" staff only "))
                        {
                            StaffOnly.StartUp();
                        }
                        else if (action.Contains(" bar "))
                        {
                            Bar.StartUp(player);
                        }
                        else if (action.Contains(" vip lounge "))
                        {
                            VIPlounge.StartUp(player);
                        }
                        else if (action.Contains(" shop "))
                        {
                            Shop.StartUp(player);
                        }
                    }
                    // Checking for inputs relating to user wanting to play a game
                    else if (Array.Exists(games, x => action.Contains(x)))
                    {
                        if (action.Contains(" roulette "))
                        {
                            Roulette.StartUp(player);
                        }
                        else if (action.Contains(" slot ") || action.Contains(" slots "))
                        {
                            Slots.StartUp(player);
                        }
                        else if (action.Contains(" blackjack "))
                        {
                            Blackjack.StartUp(player);
                        }
                    }
                    else
                    {
                        invalid = true;
                    }
                }
                // Checks whether the user is answering a question
                else if (question)
                {
                    // Checks whether user is saying no to the question
                    if (Array.Exists(disconfirm, x => action.Trim(' ') == x))
                    {
                        repeat = false;
                        return UserInput.Disconfirm;
                    }
                    // Checks whether user is saying yes to the question
                    else if (Array.Exists(confirm, x => action.Trim(' ') == x))
                    {
                        repeat = false;
                        return UserInput.Confirm;
                    }
                    // Occurs for invalid responses to the question
                    else
                    {
                        Console.WriteLine("Invalid input!\n" +
                                          "If you are asked a direct question, please reply with a one word response, e.g. \"yes\" or \"no\".");
                    }
                }
                // Checking for user inputs containings words from the first two arrays inputted to the method
                else if (nouns1 != null && Array.Exists(nouns1, x => action.Contains(x)))
                {
                    if (verbs1 != null && Array.Exists(verbs1, x => action.Contains(x)))
                    {
                        repeat = false;
                        return UserInput.ContainsFirstWords;
                    }
                    else
                    {
                        invalid = true;
                    }

                }
                // Checking for user inputs containings words from the second two arrays inputted to the method
                else if (nouns2 != null && Array.Exists(nouns2, x => action.Contains(x)))
                {
                    if (verbs2 != null && Array.Exists(verbs2, x => action.Contains(x)))
                    {
                        repeat = false;
                        return UserInput.ContainsSecondWords;
                    }
                    else
                    {
                        invalid = true;
                    }
                }
                // Checking for user inputs containings words from the third two arrays inputted to the method
                else if (nouns3 != null && Array.Exists(nouns3, x => action.Contains(x)))
                {
                    if (verbs3 != null && Array.Exists(verbs3, x => action.Contains(x)))
                    {
                        repeat = false;
                        return UserInput.ContainsThirdWords;
                    }
                    else
                    {
                        invalid = true;
                    }
                }
                else
                {
                    invalid = true;
                }

                // Occurs if user input is not recognised by any of the above possibilities
                if (invalid)
                {
                    Console.WriteLine("Invalid input!\n" +
                                      "Please make sure that your input contains a key verb and a key noun.\n" +
                                      "Both of these should be understood by the program in the current context of the situation.");
                }
            } while (repeat);

            return UserInput.Null;
        }
    }
}