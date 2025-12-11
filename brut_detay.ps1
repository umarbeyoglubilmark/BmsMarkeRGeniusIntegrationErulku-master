$content = Get-Content 'Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt' -Raw

# Her bir kaydi ayri isle
$kayitPattern = '\{\s*"id":\s*\d+,\s*"posDocumentId"[\s\S]*?"documentType":\s*(\d+)[\s\S]*?"total":\s*([\d.-]+)[\s\S]*?"lines":\s*\[([\s\S]*?)\](?=,\s*"payments")'
$kayitlar = [regex]::Matches($content, $kayitPattern)

$satisTotalPrice = 0
$satisTaxableTotal = 0
$satisTotal = 0
$satisAdet = 0

foreach($kayit in $kayitlar) {
    $docType = [int]$kayit.Groups[1].Value
    $total = [decimal]$kayit.Groups[2].Value
    $linesContent = $kayit.Groups[3].Value

    if($docType -eq 1) {
        $satisTotal += $total

        $totalPrices = [regex]::Matches($linesContent, '"totalPrice":\s*([\d.-]+)')
        foreach($tp in $totalPrices) {
            $satisTotalPrice += [decimal]$tp.Groups[1].Value
        }

        $taxableTotals = [regex]::Matches($linesContent, '"taxableTotal":\s*([\d.-]+)')
        foreach($tt in $taxableTotals) {
            $satisTaxableTotal += [decimal]$tt.Groups[1].Value
        }

        $satisAdet++
    }
}

Write-Host "=== SATIS ANALIZI (documentType=1) ==="
Write-Host "Satis Kayit Sayisi: $satisAdet"
Write-Host ""
Write-Host "total (fis toplami): $($satisTotal.ToString('N2')) TL"
Write-Host "totalPrice (satir toplami): $($satisTotalPrice.ToString('N2')) TL"
Write-Host "taxableTotal (vergilenebilir): $($satisTaxableTotal.ToString('N2')) TL"
Write-Host ""
Write-Host "Hedef: 422.452,00 TL"
Write-Host "Fark (total): $(($satisTotal - 422452).ToString('N2')) TL"
Write-Host "Fark (totalPrice): $(($satisTotalPrice - 422452).ToString('N2')) TL"
