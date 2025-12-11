$content = Get-Content 'Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt' -Raw

$lines = $content -split "`n"

$currentDocType = 0
$satisTotalPrice = 0
$satisDiscountTotal = 0

foreach($line in $lines) {
    if($line -match '"documentType":\s*(\d+)') {
        $currentDocType = [int]$matches[1]
    }

    if($currentDocType -eq 1) {
        if($line -match '"totalPrice":\s*([\d.-]+)') {
            $satisTotalPrice += [decimal]$matches[1]
        }
        if($line -match '"discountTotal":\s*([\d.-]+)') {
            $satisDiscountTotal += [decimal]$matches[1]
        }
    }
}

$brutIndirimsiz = $satisTotalPrice + $satisDiscountTotal

Write-Host "=== SATIS BRUT ANALIZI ==="
Write-Host ""
Write-Host "Indirimli Toplam (totalPrice):    $($satisTotalPrice.ToString('N2')) TL"
Write-Host "Toplam Indirim (discountTotal):   $($satisDiscountTotal.ToString('N2')) TL"
Write-Host "----------------------------------------"
Write-Host "Indirimsiz Brut Toplam:           $($brutIndirimsiz.ToString('N2')) TL"
Write-Host ""
Write-Host "=== KARSILASTIRMA ==="
Write-Host "Beklenen Indirimsiz:  422.452,00 TL"
Write-Host "Hesaplanan:           $($brutIndirimsiz.ToString('N2')) TL"
Write-Host "Fark:                 $(($brutIndirimsiz - 422452).ToString('N2')) TL"
