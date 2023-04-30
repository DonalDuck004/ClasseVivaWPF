from glob import glob
import re

files = glob("**/*.xaml", recursive=True)

color_map = {"CVAbsencesAbsent":"#FFD05A50","CVAbsencesCalendarHasEventFont":"#FFFFFFFF","CVAbsencesCalendarNoEventFont":"#FF000000","CVAbsencesEarlyExit":"#FFDBB73B","CVAbsencesFont":"#FFFFFFFF","CVAbsencesLate":"#FFEB9850","CVAbsencesPartiallyAbsent":"#FF0080B8","CVAbsencesPresent":"#FF83B588","CVAccountBubble":"#FF16A085","CVAccountBubbleFont":"#FFFFFFFF","CVBackIcon":"#FFFFFFFF","CVButton":"#FFC62828","CVCalendar":"#FFC62828","CVCheckBoxEllipseBackgroundSelected":"#FFC62828","CVCheckBoxEllipseBackgroundUnselected":"#FF7C7C7C","CVCheckBoxEllipseSelected":"#FFC62828","CVCheckBoxEllipseUnselected":"#FFFFFFFF","CVDayBgSelected":"#FF6A96AE","CVDayBgUnselected":"#00000000","CVDayHomeContainer":"#FFF0F0F0","CVDayTextSelected":"#FFFFFFFF","CVDayTextUnselected":"#FF6E6E6E","CVExtraHeaderEllipse":"#FFFFFFFF","CVExtraInteractIcons":"#FFFFFFFF","CVGenericBackground":"#FFFFFFFF","CVGenericFont":"#FF000000","CVGenericGray":"#FF808080","CVGenericGrayFont":"#FF6E6E6E","CVGenericHeaderFont":"#FFFFFFFF","CVGenericOpaqueBackground":"#FFF0F0F0","CVGenericRed":"#FFC62828","CVGenericTextSelection":"#FFC62828","CVGradeFont":"#FFFFFFFF","CVGradeGRV2":"#FFC37F45","CVGradeInsufficient":"#FFD05A50","CVGradeInsufficientBg":"#AFD05A50","CVGradeNote":"#FF5D97B1","CVGradesFilter":"#FFF0F0F0","CVGradeSlightlyInsufficient":"#FFEB9860","CVGradeSlightlyInsufficientBg":"#AFEB9860","CVGradeSufficient":"#FF83B588","CVGradeSufficientBg":"#AF83B588","CVHeader":"#FFC62828","CVHomeCurrentDay":"#FF6A96AE","CVHr":"#FFC62828","CVMainMenuIconSelected":"#FFC62828","CVMainMenuIconUnselected":"#FF808080","CVMultiMenuFontSelected":"#FFFFFFFF","CVMultiMenuFontSlider":"#FFFFFFFF","CVMultiMenuFontUnselected":"#AFFFFFFF","CVPercentageBackground":"#FFCCCCCC","CVSettingsSectionHeader":"#FFC62828","CVSettingsText":"#FF000000","CVSpinner":"#FF0000FF","CVTextBoxBackground":"#FFFFFFFF","CVUri":"#FFC62828"}

for file in files:
    with open(file, "r", encoding="utf8") as f:
        content =  f.read()
        for setter in reversed(list(re.finditer(r"(\S+=\"\{themes:ThemeBinding [^\}]+\}\")", content))):
            span = setter.span()
            setter = setter.group(0)
            p = setter.split("=")[0]

            path = r.group(1).strip("\"}") if (r := re.search(r"Path=(\S+)", setter)) else p.split(" ")[1]
            content = content[:span[0]] + f"d:{p}=\"{color_map[path]}\" {setter}" + content[span[1]:]

    with open(file, "w", encoding="utf8") as f:
        f.write(content)
