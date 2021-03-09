using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace tic_tac_toe {
    class AI {
        private const int MINIMAX_MAX_DEPTH = 5;

        private Player maximizer;
        private Player minimizer;
        GameGrid gameGrid;


        /// returns 1000 if the testingPlayer wins
        /// returns 100 if the testingPlayer checkmates
        /// returns 10 if the testingPlayer checks
        
        private int Test(in List<Player> line, Player testingPlayer, int result) {
            if (result == Int32.MaxValue) return result;

            int length = 0; //the longest continuous length of testingPlayer
            int possibleLength = 0; //length with a single NONE replaced by testingPlayer
            

            foreach(Player p in line) {
                if (p == testingPlayer) {
                    length++;
                    possibleLength++;
                }else if(p == Player.NONE) {
                    possibleLength = length + 1;
                    length = 0;
                } else {
                    length = 0;
                    possibleLength = 0;
                }

                if (length >= GameGrid.WIN_CONDITION) return 1000;
                if (possibleLength >= GameGrid.WIN_CONDITION) {
                    if (result == 0) result = 10;
                    else return 100;
                    possibleLength = length;
                }
            }

            return result;
        }
        
        public int Evaluate(in List<List<Player>> board, Player nextPlayer) {
            int O = 0, X = 0;
            List<Player> temp;

            //vertical
            for(int i = 0; i < board.Count; i++) {
                O = Test(board[i], Player.O, O);
                X = Test(board[i], Player.X, X);
            }
            //horizontal
            for (int i = 0; i < board.Count; i++) {
                temp = new List<Player>(board[i].Count);
                for (int j = 0; j < board[i].Count; j++) {
                    temp.Add(board[j][i]);                    
                }

                O = Test(temp, Player.O, O);
                X = Test(temp, Player.X, X);
            }
            //top-left to bot-right
            temp = new List<Player>(board.Count);
            for (int i = 0; i < board.Count; i++) {
                temp.Add(board[i][i]);                        
            }
            O = Test(temp, Player.O, O);
            X = Test(temp, Player.X, X);

            //top-right to bot-left
            temp = new List<Player>(board.Count);
            for (int i = 0; i < board.Count; i++) {
                temp.Add(board[i][board.Count - i - 1]);
            }
            O = Test(temp, Player.O, O);
            X = Test(temp, Player.X, X);


            //mark the opponent as negative
            if (maximizer == Player.O) X = -X;
            else O = -O;

            //return
            if (Math.Abs(O) > Math.Abs(X)) return O;
            else if (Math.Abs(X) > Math.Abs(O)) return X;
            else if (nextPlayer == Player.O) return O;
            else return X;
        }


        private int Minimax(int depth, int maxDepth, int movesLeft, List<List<Player>> board) {
            //terminal nodes
            if (Evaluate(board, maximizer) == 1000 || Evaluate(board, minimizer) == 1000) return 1000;
            if (Evaluate(board, maximizer) == -1000 || Evaluate(board, minimizer) == -1000) return -1000;
            if (depth == maxDepth || movesLeft == 0) {
                if (depth % 2 == 1) return Evaluate(board, minimizer);
                else return Evaluate(board, maximizer);
            }

            int temp;
            if(depth % 2 == 1) { //min
                int min = Int32.MaxValue;
                for (int x = 0; x < GameGrid.CELL_COUNT; x++) {
                    for (int y = 0; y < GameGrid.CELL_COUNT; y++) {
                        if (board[x][y] == Player.NONE) {
                            board[x][y] = minimizer;
                            temp = Minimax(depth + 1, maxDepth, movesLeft - 1, board);
                            if (temp < min) min = temp;
                            board[x][y] = Player.NONE;
                        }
                    }
                }
                return min;
            } else { //max
                int max = Int32.MinValue;
                for (int x = 0; x < GameGrid.CELL_COUNT; x++) {
                    for (int y = 0; y < GameGrid.CELL_COUNT; y++) {
                        if (board[x][y] == Player.NONE) {
                            board[x][y] = maximizer;
                            temp = Minimax(depth + 1, maxDepth, movesLeft - 1, board);
                            if (temp > max) max = temp;
                            board[x][y] = Player.NONE;
                        }
                    }
                }
                return max;
            }
        }

        public KeyValuePair<int,int> Move() {
            int bestMove = Int32.MinValue;
            int minimax;
            KeyValuePair<int,int> result = new KeyValuePair<int, int>(-1, -1);

            for (int x = 0; x < GameGrid.CELL_COUNT; x++) {
                for(int y = 0; y < GameGrid.CELL_COUNT; y++) {
                    if(gameGrid.gameBoard[x][y] == Player.NONE) {
                        List<List<Player>> temp = new List<List<Player>>(gameGrid.gameBoard);
                        temp[x][y] = maximizer;
                        minimax = Minimax(1, MINIMAX_MAX_DEPTH, gameGrid.MovesLeft() - 1, temp);
                        if (minimax >= bestMove) {
                            result = new KeyValuePair<int, int>(x, y);
                            bestMove = minimax;
                        }
                        temp[x][y] = Player.NONE;
                    }
                }
            }

            return result;
        }

        public AI(Player maximizer, GameGrid gameGrid) {
            this.maximizer = maximizer;
            if (maximizer == Player.O) minimizer = Player.X;
            else minimizer = Player.O;
            this.gameGrid = gameGrid;
        }
    }
}
