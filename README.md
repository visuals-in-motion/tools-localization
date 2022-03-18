# Описание

Библиотека для возможности локализации проекта. Настройка локализации осуществляется в гугл таблицах. После чего данные из таблицы загружаются в проект.

# Поддерживаемые платформы

На Windows доступна загрузка данных из гугл таблиц, а также загрузка значений в реальном времени. На Mac OS для изменения значений нужно отредактировать файлы локализации вручную и нажать кнопку загрузки.

# Подготовка данных для загрузки

Для начала создайте документ гугл таблиц по следующему приципу: первый столбец - названия ключей, второй - тип значений (text, sprite, texture, sound), все последующие - переводы.

![image](https://user-images.githubusercontent.com/52681127/159005409-38995b6a-f470-47c7-bd93-644bebf5c391.png)

Доступно 3 типа данных: 
- text (текст)
- sprite (путь к изображению относительно папки Streaming Assets)
- texture (путь к изображению относительно папки Streaming Assets)
- sound (путь к аудио клипу относительно папки Streaming Assets)

Для удобства навигации можно создать несколько листов. Все они будут загружены в проект.

Вставьте URL таблицы в файл настроек Assets -> Resources -> Localization:

![image](https://user-images.githubusercontent.com/52681127/158789339-737a4af6-2093-450e-baf1-8ec1883e2c34.png)

## Загрузка данных

Для загрузки данных выберете в верхнем меню Visuals -> Localization -> Import Google Sheets

![image](https://user-images.githubusercontent.com/52681127/159005781-c9c0e8ea-09a2-494e-92e2-bcf1a3de74d8.png)

Все файлы локализации хранятся в Assets -> Streaming Assets -> Localization.

В проекте на Windows во время загрузки скачиваются листы таблицы, обновляются файлы локализации и данные. В проекте на Mac OS только обновляются данные на основе файлов локализации.

## Просмотр таблицы

Для того, чтобы открыть таблицу в браузере, выберете в верхнем меню Visuals -> Localization -> Open Google Sheets

![image](https://user-images.githubusercontent.com/52681127/159005817-3c2fc92e-8db9-4ef3-8fb4-b4fbae407e4c.png)

## Очистка всех данных таблицы


![image](https://user-images.githubusercontent.com/52681127/159005888-8141202a-8042-4289-b97b-459569b63e3e.png)


# Основные возможности

## Компоненты

Все компоненты отображают только те категории и ключи, которые соответствуют текущему типу данных.

### VisualsLocalizationText

Для переводов текстов. Для локализации неоходимо добавить этот компонент на объект, на котором уже есть либо компонент Text, либо TextMeshPro. 

### VisualsLocalizationImage

Для замены спрайтов. Для локализации неоходимо добавить этот компонент на объект, на котором уже есть компонент Image.

### VisualsLocalizationRawImage

Для замены текстур. Для локализации неоходимо добавить этот компонент на объект, на котором уже есть компонент RawImage.

### VisualsLocalizationAudio

Для замены аудио клипов. Для локализации неоходимо добавить этот компонент на объект, на котором уже есть компонент AudioSource.

## Методы 

### LocalizationStorage

- `AddKeyValues(string categoryName, string keyName, string type, List<string> values)` - добавить новый ключ для заданной категории
- `AddCategory(string categoryName)` - добавить новую категорию в локализацию

### Общие для всех компонентов методы

- GetValuesByKeyIndex(int index) - получить данные перевода по индексу ключа
- GetCurrentKeyIndex() - получить текущий индекс ключа
- GetCurrentKeyName() - получить текущее имя ключа
- GetCurrentCategoryAllKeys() - получить все ключи для текущей категории
- GetCurrentCategoryKeysByType(string type) - получить все ключи для текущей категории заданного типа
- GetKeyIndexByName(string keyName) - получить индекс ключа текущей категории по его имени
- GetKeyNameByIndex(int keyIndex) - получить имя ключа текущей категории по его индексу
- GetCurrentCategoryName() - получить название текущей категории
- GetCurrentCategoryIndex() - получить индекс текущей категории
- GetCurrentCategoryInfo() - получить информацию о текущей категории
- GetCurrentKeyInfo() - получить информацию о текущем ключе
- GetCurrentValuesInfo() - получить данные перевода
  
- SetKeyByIndex(int index) - установить новый ключ по индексу
- SetKeyByName(string keyName) - установить новый ключ по имени
- SetCategoryByName(string categoryName) - установить новую категорию по имени
- SetCategoryByIndex(int index) - установить новую категорию по индексу
  
### VisualsLocalizationText
  
- ReplaceSubstringInText(string newString, string separatorCharacter) - замена подстроки в тексте локализации

### VisualsLocalizationAudio

- PlayAudioSource() - начать воспроизведение на текущем языке
- StopAudioSource() - остановить воспроизведение
- GetAudioSource() - получить компонент AudioSource
  
## События
  
- LocalizationStorage.changedLanguage - событие при смене языка. Возвращает индекс текущего языка.
- VisualsLocalization.loadLocalization - событие загрузки данных локализации.

## Примеры

Вывести значение на текущем языке:

```
[SerializeField] private VisualsLocalizationText localizationtext;

private void Start()
	{
		string currentText = localizationtext.GetCurrentValuesInfo()[LocalizationStorage.GetCurrentLanguages()];
	}
```

Назначить новый ключ: `localizationtext.SetKeyByIndex(0);`
