// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreImageResizingService
{
  [HtmlTargetElement("drop-down-list", Attributes = ForAttributeName + "," + ItemsAttributeName)]
  public class DropDownListTagHelper : TagHelper
  {
    private const string ForAttributeName = "asp-for";
    private const string ItemsAttributeName = "asp-items";

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For { get; set; }

    [HtmlAttributeName(ItemsAttributeName)]
    public IEnumerable<SelectListItem> Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (this.ViewContext == null || this.For == null || this.Items == null)
        return;

      output.SuppressOutput();
      output.Content.Clear();
      output.Content.AppendHtml(this.GenerateDropDownList());
    }

    private IHtmlContent GenerateDropDownList()
    {
      TagBuilder tb = new TagBuilder("div");

      tb.AddCssClass("drop-down-list");
      tb.MergeAttribute("id", this.GetIdentity());
      tb.InnerHtml.AppendHtml(this.GenerateDropDownListItem(this.GetSelectedItem(), true));
      tb.InnerHtml.AppendHtml(this.GenerateDropDownListItems());
      tb.InnerHtml.AppendHtml(this.GenerateInput());
      return tb;
    }

    private IHtmlContent GenerateDropDownListItem(SelectListItem item, bool selected = false)
    {
      TagBuilder tb = new TagBuilder("a");

      tb.AddCssClass("drop-down-list__item");

      if (selected)
        tb.AddCssClass("drop-down-list__item--selected");

      tb.MergeAttribute("href", "#");

      if (!selected)
        tb.MergeAttribute("data-value", item?.Value);

      tb.InnerHtml.AppendHtml(item?.Text);
      return tb;
    }

    private IHtmlContent GenerateDropDownListItems()
    {
      TagBuilder tb = new TagBuilder("div");

      tb.AddCssClass("drop-down-list__items");

      foreach (SelectListItem item in this.Items)
        tb.InnerHtml.AppendHtml(this.GenerateDropDownListItem(item));

      return tb;
    }

    private IHtmlContent GenerateInput()
    {
      TagBuilder tb = new TagBuilder("input");

      tb.TagRenderMode = TagRenderMode.SelfClosing;
      tb.MergeAttribute("name", this.GetIdentity());
      tb.MergeAttribute("type", "hidden");
      tb.MergeAttribute("value", this.GetSelectedItem()?.Value);
      return tb;
    }

    private string GetIdentity()
    {
      return this.For.Name;
    }

    protected string GetValue()
    {
      ModelStateEntry modelState;

      if (this.ViewContext.ModelState.TryGetValue(this.GetIdentity(), out modelState) && !string.IsNullOrEmpty(modelState.AttemptedValue))
        return modelState.AttemptedValue;

      return this.For.Model == null ? null : this.For.Model.ToString();
    }

    private SelectListItem GetSelectedItem()
    {
      string value = this.GetValue();
      SelectListItem item = null;

      if (!string.IsNullOrEmpty(value))
        item = this.Items.FirstOrDefault(i => i.Value == value);

      if (item == null)
        if (this.For.Model != null)
          item = this.Items.FirstOrDefault(i => i.Value == this.For.Model.ToString());

      if (item == null)
        item = this.Items.FirstOrDefault();

      return item;
    }
  }
}