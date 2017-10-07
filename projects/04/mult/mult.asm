// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)

// Put your code here.


// Higher level view:
// some counter, i, and the answer, initialise at 0
@i
M=0			
@R2
M=0
// while R3 < R1, add R0 to R2
(LOOP)
//									Do the Comparison
@i
D=M
@R1
D=D-M
@END
D;JGE
//									Now the addition part
@R2
D=M
@R0
D=D+M
@R2
M=D
// 									Finally increment R3, and loop
@i
M=M+1
@LOOP
0;JMP
(END)                           	//End loop