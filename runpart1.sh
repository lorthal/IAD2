#!/bin/bash

p="/mnt/d/Semestr 6/IAD/Zad2"

n=2
e=1000
l=0.6
s=3

while (( $n <= 20 ));
do
    "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "neural_gas" -o "$n""_neural_gas_square_circle_output.txt" -p "square_filled -3 0 1 150 circle 3 0 2 150"
    "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "neural_gas" -o "$n""_neural_gas_square_output.txt" -p "square_filled 0 0 3 300"

    "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "kohonen" -o "$n""_kohonen_square_circle_output.txt" -p "square_filled -3 0 1 150 circle 3 0 2 150"
    "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "kohonen" -o "$n""_kohonen_square_output.txt" -p "square_filled 0 0 3 300"
    n=$((n+2))
done
