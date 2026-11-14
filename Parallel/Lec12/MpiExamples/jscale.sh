#!/bin/bash
# Масштабованість методу Якобі: медіана 5 запусків конфігурації.
# Аргументи: n ітерацій [weak] (weak – n на один ранг).
median() {       # медіана 5 запусків за часом (4-й стовпець)
    for k in 1 2 3 4 5; do "$@"; done | sort -g -k4 | sed -n 3p
}
export OMP_NUM_THREADS=1
for np in 1 2 4 8 16; do
    opts=""; [ $np -gt 8 ] && opts="--use-hwthread-cpus"
    n=$1; [ "$3" == weak ] && n=$(($1 * np))
    median mpirun -np $np $opts ./build/jacobi $n $2
done
