namespace GELHelper.Data
{
    public static class FunctionManager
    {
        public static object getElementsFunction = new
        {
            type = "function",
            function = new
            {
                name = "get_element_by_category",
                description = "Get all elements of a specific category or familyName or familytype or Id.",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        level = new
                        {
                            type = "string",
                            description = "The level of the elements to get."
                        },
                        category = new
                        {
                            type = "string",
                            description = "The category of the elements to get. Return revit category name in English. カテゴリ名を指定します。"
                        },
                        familyName = new
                        {
                            type = "string",
                            description = "The family of the elements to get."
                        },
                        familyType = new
                        {
                            type = "string",
                            description = "The family type of the elements to get."
                        },
                        filter_type = new
                        {
                            type = "string",
                            description = "select or unselect the elements. 要素を選択するか選択解除するかを指定します。",
                            _enum = new[] { "select", "unselect" }
                        },
                    },
                    required = new[]
                    {
                        "filter_type"
                    }
                }
            }
        };
        public static object showElementsFunction = new
        {
            type = "function",
            function = new
            {
                name = "show_element_by_category",
                description = "Get all elements of a specific category or familyName or familytype or Id and Edit visibility of selected elements. 指定したカテゴリかファミリ名かファミリタイプで選択した要素の表示非表示の設定を変更します。",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        level = new
                        {
                            type = "string",
                            description = "The level of the elements to get."
                        },
                        category = new
                        {
                            type = "string",
                            description = "The category of the elements to get. Return revit category name in English. カテゴリ名を指定します。"
                        },
                        familyName = new
                        {
                            type = "string",
                            description = "The family of the elements to get."
                        },
                        familyType = new
                        {
                            type = "string",
                            description = "The family type of the elements to get."
                        },
                        filter_type = new
                        {
                            type = "string",
                            description = "Identify the instruction hide or show. 表示をしたいのか非表示にしたいのかを指定します。",
                            _enum = new[] { "show", "hide" }
                        },
                    },
                    required = new[]
                    {
                        "filter_type"
                    }
                }
            }
        };
        public static object calculateElementsFunction = new
        {
            type = "function",
            function = new
            {
                name = "calculate_element_by_category",
                description = "Get all elements of a specific category or familyName or familytype or Id and count of selected elements. 指定したカテゴリかファミリ名かファミリタイプで選択した要素の数量を数えます。",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        level = new
                        {
                            type = "string",
                            description = "The level of the elements to get."
                        },
                        category = new
                        {
                            type = "string",
                            description = "The category of the elements to get. Return revit category name in English. カテゴリ名を指定します。"
                        },
                        familyName = new
                        {
                            type = "string",
                            description = "The family of the elements to get."
                        },
                        familyType = new
                        {
                            type = "string",
                            description = "The family type of the elements to get."
                        },
                        view = new
                        {
                            type = "string",
                            description = "The view to get elements.ビューを指定して要素を取得します。",
                            _enum = new[] { "activeView", "none"} 
                        },
                        
                    },
                    required = new[]
                    {
                        "filter_type"
                    }
                }
            }
        };
    }
}