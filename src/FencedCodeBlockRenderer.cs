// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Markdig.Helpers;
using Markdig.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using ColorCode.Console;
using ColorCode.Styling;
using Markdig.Syntax.Inlines;

namespace Microsoft.PowerShell.MarkdownRender
{
    /// <summary>
    /// Renderer for adding VT100 escape sequences for code blocks with language type.
    /// </summary>
    internal class FencedCodeBlockRenderer : VT100ObjectRenderer<FencedCodeBlock>
    {
        readonly VT100Formatter formatter = new(StyleDictionary.DefaultDark);
        protected override void Write(VT100Renderer renderer, FencedCodeBlock obj)
        {
            if (obj?.Lines.Lines != null)
            {
                var text = string.Join(obj.NewLine.AsString(), new ArraySegment<StringLine>(obj.Lines.Lines, 0, obj.Lines.Count));
                var lang = string.IsNullOrEmpty(obj.Info) ? null : ColorCode.Languages.FindById(obj.Info);
                if (lang is null)
                {
                    renderer.WriteLine("++++ Code ".PadRight(Console.WindowWidth, '+'));
                }
                else
                {
                    renderer.WriteLine($"++++ {lang.Name} Code ".PadRight(Console.WindowWidth, '+'));
                }
                lang ??= ColorCode.Languages.Python; // just treat it as python. 
                text = formatter.GetVT100String(text, lang);
                renderer.WriteLine(text);
                renderer.WriteLine(new string('+', Console.WindowWidth));
            }
        }
    }
}
