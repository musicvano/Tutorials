# Локальний реєстр образів і кластер kind, що бере образи з нього.
docker run -d --restart=always -p 127.0.0.1:5001:5000 `
    --name pro16-registry registry:2
kind create cluster --config kind.yaml
# Вузли kind – контейнери: для них localhost:5001 – це реєстр.
$dir = "/etc/containerd/certs.d/localhost:5001"
foreach ($node in kind get nodes --name pro16) {
    docker exec $node mkdir -p $dir
    '[host."http://pro16-registry:5000"]' |
        docker exec -i $node cp /dev/stdin "$dir/hosts.toml"
}
docker network connect kind pro16-registry
kubectl get nodes
