#!/bin/bash

p="/mnt/d/Semestr 6/IAD/Zad2"
r="/mnt/d/Semestr 6/IAD/Zad2Res/"

np="NeuralGas/"
k="Kohonen/"
o="One/"
t="Two/"

n=20
e=1000

makedirs() 
{
    mkdir "$r""$np""$o""/l_""$l""_s_""$s/" -p
    mkdir "$r""$k""$o""/l_""$l""_s_""$s/" -p
    mkdir "$r""$np""$t""/l_""$l""_s_""$s/" -p
    mkdir "$r""$k""$t""/l_""$l""_s_""$s/" -p
}

runprogs()
{
    makedirs

    for i in {1..100};
    do
        "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "neural_gas" -o "$np""$o""/l_""$l""_s_""$s/""$i""_neural_gas_square_output.txt" -p "square_filled 0 0 3 300"    
        "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "neural_gas" -o "$np""$t""/l_""$l""_s_""$s/""$i""_neural_gas_square_circle_output.txt" -p "square_filled -3 0 1 150 circle 3 0 2 150"

        "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "kohonen" -o "$k""$o""/l_""$l""_s_""$s/""$i""_kohonen_square_output.txt" -p "square_filled 0 0 3 300"    
        "$p""/runprog.sh" -e $e -n $n -l $l -s $s -m "kohonen" -o "$k""$t""/l_""$l""_s_""$s/""$i""_kohonen_square_circle_output.txt" -p "square_filled -3 0 1 150 circle 3 0 2 150"
    done
}

l=0.1
s=3
runprogs

l=0.5
s=3
runprogs

l=0.9
s=3
runprogs

l=0.1
s=6
runprogs

l=0.5
s=6
runprogs

l=0.9
s=6
runprogs

l=0.1
s=9
runprogs

l=0.5
s=9
runprogs

l=0.9
s=9
runprogs