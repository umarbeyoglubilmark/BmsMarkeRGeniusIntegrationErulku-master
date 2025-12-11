$content = Get-Content 'Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt' -Raw

# Tum JSON bloklarini ayir - "datas": ile baslayan bolumler
$pattern = '"datas":\s*\[(.*?)\](?=,\s*"paging"|\s*})'
$datasMatches = [regex]::Matches($content, $pattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)

Write-Host "Bulunan datas blogu sayisi: $($datasMatches.Count)"

# Her bir satis kaydini ayri ayri isle
# documentType: 1 = Satis, 3 = Iade

$satisPattern = '"documentType":\s*1[\s\S]*?"lines":\s*\[([\s\S]*?)\](?=,\s*"payments")'
$iadePattern = '"documentType":\s*3[\s\S]*?"lines":\s*\[([\s\S]*?)\](?=,\s*"payments")'

# Daha iyi yaklasim - her bir kaydi ayri isle
$kayitPattern = '\{\s*"id":\s*\d+,\s*"posDocumentId"[\s\S]*?"documentType":\s*(\d+)[\s\S]*?"lines":\s*\[([\s\S]*?)\](?=,\s*"payments")'
$kayitlar = [regex]::Matches($content, $kayitPattern)

$satisBrut = 0
$iadeBrut = 0
$satisAdet = 0
$iadeAdet = 0

foreach($kayit in $kayitlar) {
    $docType = [int]$kayit.Groups[1].Value
    $linesContent = $kayit.Groups[2].Value

    $totalPrices = [regex]::Matches($linesContent, '"totalPrice":\s*([\d.-]+)')
    $kayitToplam = 0
    foreach($tp in $totalPrices) {
        $kayitToplam += [decimal]$tp.Groups[1].Value
    }

    if($docType -eq 1) {
        $satisBrut += $kayitToplam
        $satisAdet++
    } elseif($docType -eq 3) {
        $iadeBrut += $kayitToplam
        $iadeAdet++
    }
}

Write-Host ""
Write-Host "=== SATIS (documentType=1) ==="
Write-Host "Satis Adet: $satisAdet"
Write-Host "Satis Brut Toplam: $($satisBrut.ToString('N2')) TL"
Write-Host ""
Write-Host "=== IADE (documentType=3) ==="
Write-Host "Iade Adet: $iadeAdet"
Write-Host "Iade Brut Toplam: $($iadeBrut.ToString('N2')) TL"
Write-Host ""
Write-Host "=== NET ==="
Write-Host "Net Brut Toplam (Satis - Iade): $(($satisBrut - $iadeBrut).ToString('N2')) TL"
