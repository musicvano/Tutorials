#!/bin/bash
# Серія запусків для 1, 2, 4, 8, 16 процесів і таблиця S та E.
for np in 1 2 4 8 16; do
    # 16 рангів на 8 ядрах: логічні процесори SMT як окремі слоти.
    opts=""; [ $np -gt 8 ] && opts="--use-hwthread-cpus"
    mpirun -np $np $opts ./build/pi
done | awk 'NR == 1 { t1 = $2
                      printf "%3s %8s %6s %5s %16s %8s\n",
                             "np", "T, с", "S", "E", "pi", "похибка" }
            { s = t1 / $2
              printf "%3d %8.3f %6.2f %4.0f%% %16s %8s\n",
                     $1, $2, s, 100 * s / $1, $3, $4 }'
