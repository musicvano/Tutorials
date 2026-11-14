#!/bin/bash
# Конфігурації «ранги × потоки» для 8 ядер: кожен ранг отримує
# PE=t ядер (--map-by slot:PE=t), а його потоки OpenMP – по ядру.
median() {       # медіана 5 запусків за часом (4-й стовпець)
    for k in 1 2 3 4 5; do "$@"; done | sort -g -k4 | sed -n 3p
}
export OMP_PLACES=cores OMP_PROC_BIND=close
for r in 8 4 2 1; do
    t=$((8 / r))
    OMP_NUM_THREADS=$t median mpirun -np $r --map-by slot:PE=$t \
        -x OMP_NUM_THREADS ./build/jacobi "$@"
done
