#!/bin/bash

# D:\Semestr 6\IAD\Zad2\Zad2\Zadanie2\bin\Debug
programpath='/mnt/d/Semestr 6/IAD/Zad2/Zad2/Zadanie2/bin/Debug/Zadanie2.exe'
epochs="100"
neurons="100"
learningRate="0.6"
simga="3"
outputFilename="output.txt"
shape=""

# Available shapes
# sector (int)centerX (int)centerY (int)halfLength (bool)xAxis (int) points
# square(_filled) (int)centerX (int)centerY (int)halfLength (int) points
# circle(circumference) (int)centerX (int)centerY (int)radius (int) points

method="kohonen"
# Available methods
# kohonen
# neural_gas

# Parse args
scriptname=$(basename "$0")
for arg in "$@"; do
    if [[ "$arg" == '--help' ]]; then
        exit 0
    fi
done

while (("$#")); do
    case "$1" in
        -l|--learningRate)
            learningRate=$2
            shift 2
            ;;
        -e|--epochs)
            epochs=$2
            shift 2
            ;;
        -n|--neurons)
            neurons=$2
            shift 2
            ;;
        -s|--sigma)
            sigma=$2
            shift 2
            ;;
        -o|--output)
            outputFilename=$2
            shift 2
            ;;
        -m|--method)
            method=$2
            shift 2
            ;;
        -p|--pattern)
            shape="$2"
            shift 2
            ;;
        --) #end parsing
            shift
            break
            ;;
        -*|--*=) # unsupported flags
            echo "Error: Unsupported flag $1" >&2
            exit 1
            ;;
        *) # preserve positional arguments
            PARAM="$PARAMS $1"
            shift
            ;;
    esac
done

eval set -- "$PARAMS"

echo $PARAMS

# echo "$programpath" "$epochs" "$neurons" "$learningRate" "$sigma" "$method" "$outputFilename" "$shape"

"$programpath" $epochs $neurons $learningRate $sigma $method $outputFilename $shape
