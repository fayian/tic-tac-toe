using System;
using System.Collections.Generic;
using System.Text;

namespace tic_tac_toe {
    class AI {
        private int winCondition;
        private Player maximizer;
        private List<List<Player>> gameBoard;

        /// returns INFINITY if the testingPlayer wins
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

                if (length >= winCondition) return Int32.MaxValue;
                if (possibleLength >= winCondition) {
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

        public AI(Player maximizer, int winCondition, List<List<Player>> gameBoard) {
            this.maximizer = maximizer;
            this.winCondition = winCondition;
            this.gameBoard = gameBoard;
        }
    }
}
