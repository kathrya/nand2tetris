// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.

// Higher level thoughts:
// A continuous loop running through all pixels, that checks at each step if keyboard is
// pressed, then swap from blackening loop to whitening loop

// White Loop: Check to see if should be black loop
// Whiten bit, and advance by one (resetting if needed)

// Initialise the end of the screen
@24576
D=A
@end
M=D

// Reset pixel
(RESET)
@16384
D=A
@pixel
M=D

// White Loop
(WHITE)
@24576
D=M
@BLACK
D;JNE
// Whiten Pixel
@pixel
A=M
M=0
// increment
@pixel
M=M+1
//Check if pixel > 24576, and reset
D=M
@end
D=M-D
@RESET
D;JLT
@WHITE
0;JMP


// Black Loop:
(BLACK)
@24576
D=M
@WHITE
D;JEQ
// Blacken Pixel
@pixel
A=M
M=-1
// increment
@pixel
M=M+1
//Check if pixel > 24576, and reset
D=M
@end
D=M-D
@RESET
D;JLT
@BLACK
0;JMP