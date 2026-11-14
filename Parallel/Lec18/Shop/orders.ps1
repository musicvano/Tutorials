# Три замовлення через шлюз: успішне, відмова оплати, нестача товару.
$gw = "http://localhost:5100/api/orders"
$orders = @(
    @{ customer = "olena"
       lines = @(@{ productId = 1; quantity = 1 },
                 @{ productId = 2; quantity = 2 }) },
    @{ customer = "petro"
       lines = @(@{ productId = 1; quantity = 2 }) },
    @{ customer = "ivan"
       lines = @(@{ productId = 2; quantity = 50 }) }
)
foreach ($o in $orders) {
    $r = Invoke-RestMethod -Method Post $gw `
        -ContentType "application/json" `
        -Body ($o | ConvertTo-Json -Depth 3)
    "{0,-6} {1,7} грн  {2,-9} {3}" -f $r.customer, $r.total,
        $r.status, $r.reason
}
