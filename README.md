# Sudoku_Solver (Open as RAW)
Solves sudoku puzzles using an algorithm that I made

I essentially have a 2D array of Nums (Num is an object I created) where each Num consists of a value and a list with the numbers 1 through 9. If the box on the Sudoku puzzle has a value, then value is set to that number. If it is blank, value is set to 0. 

The next part is taking every Num that isn't 0 (every Num that has a value) and removing the value of that Num from all other Nums in the same row and column. If a Num's list reaches a point where it only has 1 possibility, then that possibility is set to the value. 

The same thing is done for each 3x3 square (remember, each 3x3 square must contain the numbers 1 through 9) where any Num in a square removes the possibility of that value for all Nums in the same square. So if the a square looks like this (see below)...
~~~
4 _ _

_ 1 _

_ _ _
~~~
...then all the blank Num's would have the possibilities 1 and 4 removed from their possibilities list because those numbers cannot exist there.

Eventually you may reach a point where you need to guess a number so the board will guess a square with a small number of possibilities left in its list and then run through the entire process that you saw above. If another guess is necessary then it will repeat recursivly. If a guess is wrong it will back track to the board before the guess and try a different number.

That's it :)
