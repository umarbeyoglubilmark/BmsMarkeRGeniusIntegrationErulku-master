$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw
$pattern = '"total": ([\d.-]+)'
$m = [regex]::Matches($content, $pattern)
$sum = 0
foreach($match in $m) {
    $sum += [decimal]$match.Groups[1].Value
}
Write-Host "Toplam Invoice Sayisi:" $m.Count
Write-Host "Toplam Satis Tutari:" $sum.ToString("N2") "TL"
