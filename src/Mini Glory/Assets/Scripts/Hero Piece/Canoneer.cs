using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canoneer : HeroPiece
{
    public override string ToString()
    {
        return "Canoneer";
    }
    // Start is called before the first frame update
    void Awake()
    {
        life = 1;
        nextUlti = 5;
        ultiCounter = 0;
    }

    private List<Vector2Int> GetJumpableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        //Up
        bool isBlocked = false;
        for (int i = currentY + 1; i < tileCountY; i++)
        {
            if(board[currentX, i] != null && board[currentX, i].team != team)
            {
                if(!isBlocked)
                {
                    isBlocked = true;
                    continue;
                }

                r.Add(new Vector2Int(currentX, i));
            }
        }

        //Down
        isBlocked = false;
        for (int i = currentY - 1; i >= 0; i--)
        {
            if(board[currentX, i] != null && board[currentX, i].team != team)
            {
                if(!isBlocked)
                {
                    isBlocked = true;
                    continue;
                }

                r.Add(new Vector2Int(currentX, i));
            }
        }

        //Left
        isBlocked = false;
        for (int i = currentX - 1; i >= 0; i--)
        {
            if(board[i, currentY] != null && board[i, currentY].team != team)
            {
                if(!isBlocked)
                {
                    isBlocked = true;
                    continue;
                }

                r.Add(new Vector2Int(i, currentY));
            }
        }

        //Right
        isBlocked = false;
        for (int i = currentX + 1; i < tileCountX; i++)
        {
            if(board[i, currentY] != null && board[i, currentY].team != team)
            {
                if(!isBlocked)
                {
                    isBlocked = true;
                    continue;
                }

                r.Add(new Vector2Int(i, currentY));
            }
        }

        return r;
    }

    public override void ulti(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        GameController gamectrl = FindObjectOfType<GameController>();
        for (int y = currentY - 1, x = currentX; y <= currentY + 1; y++)
        {
            if(y >= 0 && y < tileCountY && (x != currentX || y != currentY))
            {
                if (board[x, y] != null)
                {
                    if (board[x, y].team != team)
                    {
                        if (board[x, y].team == 1)
                        {
                            gamectrl.Black_Defeat(board[x, y], x, y);
                        }   else
                        {
                            gamectrl.White_Defeat(board[x, y], x, y);
                        }
                    }
                }
            }
        }

        for (int x = currentX - 1, y = currentY; x <= currentX + 1; x++)
        {
            if(x >= 0 && x < tileCountX && (x != currentX || y != currentY))
            {
                if (board[x, y] != null)
                {
                    if (board[x, y].team != team)
                    {
                        if (board[x, y].team == 1)
                        {
                            gamectrl.Black_Defeat(board[x, y], x, y);
                        }   else
                        {
                            gamectrl.White_Defeat(board[x, y], x, y);
                        }
                    }
                }
            }
        }
    }

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        //Up
        for (int i = currentY + 1; i < tileCountY && i <= currentY + 2; i++)
        {
            if(board[currentX, i] == null)
                r.Add(new Vector2Int(currentX, i));
            
            if(board[currentX, i] != null)
            {
                if(board[currentX, i].team != team)
                    r.Add(new Vector2Int(currentX, i));

                break;
            }
        }

        //Down
        for (int i = currentY - 1; i >= 0 && i >= currentY - 2; i--)
        {
            if(board[currentX, i] == null)
                r.Add(new Vector2Int(currentX, i));
            
            if(board[currentX, i] != null)
            {
                if(board[currentX, i].team != team)
                    r.Add(new Vector2Int(currentX, i));

                break;
            }
        }

        //Right
        for (int i = currentX + 1; i < tileCountX && i <= currentX + 2; i++)
        {
            if(board[i, currentY] == null)
                r.Add(new Vector2Int(i, currentY));
            
            if(board[i, currentY] != null)
            {
                if(board[i, currentY].team != team)
                    r.Add(new Vector2Int(i, currentY));

                break;
            }
        }

        //Left
        for (int i = currentX - 1; i >= 0 && i >= currentX - 2; i--)
        {
            if(board[i, currentY] == null)
                r.Add(new Vector2Int(i, currentY));
            
            if(board[i, currentY] != null)
            {
                if(board[i, currentY].team != team)
                    r.Add(new Vector2Int(i, currentY));

                break;
            }
        }

        //Top Right    
        for(int x = currentX + 1, y = currentY + 1; x < tileCountX && y < tileCountY && x <= currentX + 2 && y <= currentY + 2; x++, y++)
        {
            if(board[x,y] == null)
                r.Add(new Vector2Int(x,y));

            else
            {
                if(board[x,y].team != team)
                    r.Add(new Vector2Int(x,y));
                
                break;
            }    
        }

        //Top Left  
        for(int x = currentX - 1, y = currentY + 1; x >= 0 && y < tileCountY && x >= currentX - 2 && y <= currentY + 2; x--, y++)
        {
            if(board[x,y] == null)
                r.Add(new Vector2Int(x,y));

            else
            {
                if(board[x,y].team != team)
                    r.Add(new Vector2Int(x,y));
                
                break;
            }    
        }

        //Bottom Right    
        for(int x = currentX + 1, y = currentY - 1; x < tileCountX && y >= 0 && x <= currentX + 2 && y >= currentY - 2; x++, y--)
        {
            if(board[x,y] == null)
                r.Add(new Vector2Int(x,y));

            else
            {
                if(board[x,y].team != team)
                    r.Add(new Vector2Int(x,y));
                
                break;
            }    
        }

        //Bottom Left  
        for(int x = currentX - 1, y = currentY - 1; x >= 0 && y >= 0 && x >= currentX - 2 && y >= currentY - 2; x--, y--)
        {
            if(board[x,y] == null)
                r.Add(new Vector2Int(x,y));

            else
            {
                if(board[x,y].team != team)
                    r.Add(new Vector2Int(x,y));
                
                break;
            }    
        }

        return r;
    }
}
