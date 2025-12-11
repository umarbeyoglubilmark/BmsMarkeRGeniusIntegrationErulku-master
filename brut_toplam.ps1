$content = Get-Content 'Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt' -Raw

# totalPrice değerlerini bul (satır bazında brüt fiyat - indirim öncesi)
$totalPriceMatches = [regex]::Matches($content, '"totalPrice":\s*([\d.-]+)')

$brutToplam = 0
foreach($m in $totalPriceMatches) {
    $brutToplam += [decimal]$m.Groups[1].Value
}

Write-Host "Toplam totalPrice sayisi: $($totalPriceMatches.Count)"
Write-Host "Brut Satis Toplami (totalPrice): $($brutToplam.ToString('N2')) TL"
