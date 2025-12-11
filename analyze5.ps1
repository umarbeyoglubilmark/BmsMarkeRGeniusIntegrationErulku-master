# Indirim analizi
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

$pattern = '"discountTotal":\s*(\d+)'
$matches = [regex]::Matches($content, $pattern)

$totalDiscount = 0
$count = 0

foreach($m in $matches) {
    $discount = [decimal]$m.Groups[1].Value
    if($discount -gt 0) {
        $totalDiscount += $discount
        $count++
    }
}

Write-Host "Indirimli Satir Sayisi: $count"
Write-Host "Toplam Indirim: $($totalDiscount.ToString('N2')) TL"
