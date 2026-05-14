# Timber Cut Planner Mockup

Basit bir Windows Forms tabanlı demo uygulamasıdır. Proje, kesim planlama ve üretim takibi için mockup arayüzü sunar.

## Özellikler
- Girdi (Input) ekranı
- Üretim (Production) izleme ekranı
- Basit örnek parça listesi

## Gereksinimler
- Windows
- .NET SDK 8.0 veya daha yeni bir sürüm (x64)

## Hızlı başlatma
Bu dizinde proje dosyası bulunmaktadır. Projeyi derlemek ve çalıştırmak için aşağıdaki komutları kullanabilirsiniz:

```powershell
dotnet build TimberCutPlannerMockup/TimberCutPlannerMockup.csproj -c Debug
dotnet run --project TimberCutPlannerMockup/TimberCutPlannerMockup.csproj -c Debug
```

Alternatif olarak derlenen DLL'i doğrudan çalıştırabilirsiniz:

```powershell
dotnet TimberCutPlannerMockup\\bin\\Debug\\net8.0-windows\\TimberCutPlannerMockup.dll
```

## Notlar
- GUI bir Windows Forms uygulamasıdır; uzak/headless makinelerde pencere gösterimi desteklenmeyebilir.
- Proje örnek amaçlıdır; gerçek üretim kullanımına alınmadan önce validasyon gerektirir.

## Lisans
Bu depo kişisel demo amaçlıdır.
