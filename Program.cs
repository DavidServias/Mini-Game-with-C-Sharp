using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = {"('-')", "(^-^)", "(X_X)"};
string[] foods = {"@@@@@", "$$$$$", "#####"};

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit) 
{
    Move();
    if (TerminalResized())
    {
        shouldExit = true;
        Console.WriteLine("Console was resized. Program exiting.");
    }

    if (FoodWasConsumed() )
    {
        // If this food is empty
        if (foods[food] == "")
        {
            // If all food is empty, end the came
            if (FoodIsGone() )
            {
                Console.SetCursorPosition(0, Console.WindowLeft);
                Console.WriteLine("You ate all the food!");
                shouldExit = true;
            }
            // Otherwise, choose another food.
            else
            {
                do 
                {
                    ShowFood();
                } while (foods[food] == "");
                ChangePlayer();
                if (food == 2)
                {
                    FreezePlayer();
                    
                } 
            }

        }    
        
    }; // if
    
    // Display player coordinates 
    // if (!ShouldExit) 
    // {
        // Console.SetCursorPosition(Console.WindowHeight, 0);
        // Console.WriteLine("playerX: " + playerX + "  playerY: " + playerY + " foodX: " + foodX + " foodY: " + foodY);
    // }
    

}; // end while

// Returns true if the Terminal was resized 
bool TerminalResized() 
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood() 
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Changes the player to match the food consumed
void ChangePlayer() 
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player + "   ");
}

// Temporarily stops the player from moving
void FreezePlayer() 
{
    Console.SetCursorPosition(playerX, playerY);
    Console.Write("**FROZEN**");
    System.Threading.Thread.Sleep(1000);
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player + "     ");
}


// Returns true if food was consumed. Also updates the food string.
bool FoodWasConsumed() {
    bool result = false;
    if (playerY == foodY)
    {
        // Player right edge is past food left edge
        if (playerX > foodX-player.Length)
        {
            // player left edge is colloding with the food
            if (playerX == (foodX + foods[food].Length-1) )
            {
                result = true;
                // Remove the last character from the food string
                foods[food] = foods[food].Substring(0, foods[food].Length-1);
            }
            // player right edge is colliding with food
            else if (playerX < (foodX + foods[food].Length) )
            {
                result = true;
                foods[food] = foods[food].Substring(0, foods[food].Length-1);
                foodX += 1;

            }
            
            // Display the food at the location
            Console.SetCursorPosition(foodX, foodY);
            Console.Write(foods[food]);
        }
            
    }

    return result;
}

// Checks if all the strings in foods array are empty strings, which would mean that all of the FoodIsGone
bool FoodIsGone() 
{
    bool isGone = true;
    for (int i = 0; i < foods.Length; i++) 
    {
        if (foods[i] != "")
        {
            isGone = false;
            break;
        } 
    }
    return isGone;

};

// Reads directional input from the Console and moves the player
void Move(bool DetectNondirectionalKeys = false) 
{
    int lastX = playerX;
    int lastY = playerY;
    
    switch (Console.ReadKey(true).Key) 
    {
        case ConsoleKey.UpArrow:
            playerY--; 
            break;
		case ConsoleKey.DownArrow: 
            playerY++; 
            break;
		case ConsoleKey.LeftArrow:  
            playerX--; 
            break;
		case ConsoleKey.RightArrow: 
            playerX++; 
            break;
		case ConsoleKey.Escape:
            shouldExit = true;                        
            break;
        default:
            if (DetectNondirectionalKeys)
            {
                shouldExit = true; 
            }     

            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) 
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Clears the console, displays the food and player
void InitializeGame() 
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}