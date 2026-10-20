using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineScanInspection.Help
{
    public class HalconHelper
    {
        /// <summary>
        /// 读取图片并识别（加载图片）
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="hv_WindowHandle"></param>
        public void ReadImageOcr(string imagePath,HTuple hv_WindowHandle)
        {
            // Local iconic variables 

            HObject ho_Image, ho_GrayImage, ho_BrightAll;
            HObject ho_BrightComponents, ho_LargeBrightComponents, ho_GroupedCandidates;
            HObject ho_GroupedUnion, ho_ConnectedGroups, ho_LargestTextGroup;
            HObject ho_TextROI, ho_GrayROI, ho_BrightRegion, ho_ConnectedRegions;
            HObject ho_CharacterCandidates, ho_Characters;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Col1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Col2 = new HTuple();
            HTuple hv_NumberCharacters = new HTuple(), hv_CharacterNames = new HTuple();
            HTuple hv_CharacterCount = new HTuple(), hv_OCRHandle = new HTuple();
            HTuple hv_TrainingError = new HTuple(), hv_ErrorLog = new HTuple();
            HTuple hv_Classes = new HTuple(), hv_Confidence = new HTuple();
            HTuple hv_RecognizedText = new HTuple(), hv_Success = new HTuple();
            HTuple hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_BrightAll);
            HOperatorSet.GenEmptyObj(out ho_BrightComponents);
            HOperatorSet.GenEmptyObj(out ho_LargeBrightComponents);
            HOperatorSet.GenEmptyObj(out ho_GroupedCandidates);
            HOperatorSet.GenEmptyObj(out ho_GroupedUnion);
            HOperatorSet.GenEmptyObj(out ho_ConnectedGroups);
            HOperatorSet.GenEmptyObj(out ho_LargestTextGroup);
            HOperatorSet.GenEmptyObj(out ho_TextROI);
            HOperatorSet.GenEmptyObj(out ho_GrayROI);
            HOperatorSet.GenEmptyObj(out ho_BrightRegion);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_CharacterCandidates);
            HOperatorSet.GenEmptyObj(out ho_Characters);
            try
            {
                //GLOSSED OCR 训练程序：先运行本程序，再运行识别程序。
                //处理流程：读取图像 -> 阈值分割 -> 找最大文字区域 -> 分割字符 -> 训练 OCR。
                //图片、训练文件和模型路径均直接写在代码中。

                //输入：图片路径；输出：Image 图像对象。
                ho_Image.Dispose();
                HOperatorSet.ReadImage(out ho_Image, imagePath);
                //输入：彩色 Image；输出：GrayImage 灰度图。
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                //输入：GrayImage；输出：BrightAll；150 到 255 保留亮字符。
                ho_BrightAll.Dispose();
                HOperatorSet.Threshold(ho_GrayImage, out ho_BrightAll, 150, 255);
                //输入：BrightAll；输出：BrightComponents；分离连通区域。
                ho_BrightComponents.Dispose();
                HOperatorSet.Connection(ho_BrightAll, out ho_BrightComponents);
                //输入：BrightComponents；输出：LargeBrightComponents；按面积、高度、宽度过滤小噪声。
                ho_LargeBrightComponents.Dispose();
                HOperatorSet.SelectShape(ho_BrightComponents, out ho_LargeBrightComponents,
                    ((new HTuple("area")).TupleConcat("height")).TupleConcat("width"), "and",
                    ((new HTuple(300)).TupleConcat(20)).TupleConcat(10), ((new HTuple(50000)).TupleConcat(
                    300)).TupleConcat(300));
                //输入：候选区域；输出：GroupedCandidates；横向 121、纵向 5，连接同一行字符。
                ho_GroupedCandidates.Dispose();
                HOperatorSet.DilationRectangle1(ho_LargeBrightComponents, out ho_GroupedCandidates,
                    121, 5);
                //输入：GroupedCandidates；输出：GroupedUnion；合并候选区域。
                ho_GroupedUnion.Dispose();
                HOperatorSet.Union1(ho_GroupedCandidates, out ho_GroupedUnion);
                //输入：GroupedUnion；输出：ConnectedGroups；得到合并区域。
                ho_ConnectedGroups.Dispose();
                HOperatorSet.Connection(ho_GroupedUnion, out ho_ConnectedGroups);
                //输入：ConnectedGroups；输出：LargestTextGroup；选择面积最大的区域。
                ho_LargestTextGroup.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedGroups, out ho_LargestTextGroup, "max_area",
                    70);
                //输入：最大区域；输出：Row1、Col1、Row2、Col2 边界坐标。
                hv_Row1.Dispose(); hv_Col1.Dispose(); hv_Row2.Dispose(); hv_Col2.Dispose();
                HOperatorSet.SmallestRectangle1(ho_LargestTextGroup, out hv_Row1, out hv_Col1,
                    out hv_Row2, out hv_Col2);
                //输入：四个边界坐标；输出：TextROI；生成动态文字 ROI。
                ho_TextROI.Dispose();
                HOperatorSet.GenRectangle1(out ho_TextROI, hv_Row1, hv_Col1, hv_Row2, hv_Col2);
                //输入：GrayImage 和 TextROI；输出：GrayROI；限制文字处理范围。
                ho_GrayROI.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_TextROI, out ho_GrayROI);
                //输入：GrayROI；输出：BrightRegion；再次提取亮字符。
                ho_BrightRegion.Dispose();
                HOperatorSet.Threshold(ho_GrayROI, out ho_BrightRegion, 150, 255);
                //输入：BrightRegion；输出：ConnectedRegions；分离单个字符。
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_BrightRegion, out ho_ConnectedRegions);
                //输入：ConnectedRegions；输出：CharacterCandidates；过滤不符合尺寸的区域。
                ho_CharacterCandidates.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_CharacterCandidates, (
                    (new HTuple("area")).TupleConcat("height")).TupleConcat("width"), "and",
                    ((new HTuple(5000)).TupleConcat(120)).TupleConcat(40), ((new HTuple(50000)).TupleConcat(
                    260)).TupleConcat(250));
                //输入：字符候选区域；输出：Characters；按列从左到右排序。
                ho_Characters.Dispose();
                HOperatorSet.SortRegion(ho_CharacterCandidates, out ho_Characters, "character",
                    "true", "column");
                //输入：Characters；输出：NumberCharacters；统计字符数量。
                hv_NumberCharacters.Dispose();
                HOperatorSet.CountObj(ho_Characters, out hv_NumberCharacters);
                //字符数量不是 7 时停止。
                if ((int)(new HTuple(hv_NumberCharacters.TupleNotEqual(7))) != 0)
                {
                    //输出错误信息。
                    throw new HalconException(new HTuple("Expected 7 characters, but segmented ") + hv_NumberCharacters);
                    //结束数量判断。
                }
                //输入：字符区域、原图、字符标签；输出：训练文件。
                HOperatorSet.WriteOcrTrainf(ho_Characters, ho_GrayImage, ((((((new HTuple("G")).TupleConcat(
                    "L")).TupleConcat("O")).TupleConcat("S")).TupleConcat("S")).TupleConcat(
                    "E")).TupleConcat("D"), "C:/Users/16348/Desktop/课件/实战/12.trf");
                //输入：训练文件；输出：类别名称 CharacterNames 和样本数量 CharacterCount。
                hv_CharacterNames.Dispose(); hv_CharacterCount.Dispose();
                HOperatorSet.ReadOcrTrainfNames("C:/Users/16348/Desktop/课件/实战/12.trf",
                    out hv_CharacterNames, out hv_CharacterCount);
                //输入：类别名称和 MLP 参数；输出：OCRHandle 分类器句柄。
                hv_OCRHandle.Dispose();
                HOperatorSet.CreateOcrClassMlp(8, 10, "constant", "default", hv_CharacterNames,
                    20, "normalization", 10, 42, out hv_OCRHandle);
                //输入：分类器和训练文件；200 是最大训练次数；输出训练误差和日志。
                hv_TrainingError.Dispose(); hv_ErrorLog.Dispose();
                HOperatorSet.TrainfOcrClassMlp(hv_OCRHandle, "C:/Users/16348/Desktop/课件/实战/12.trf",
                    200, 0.01, 0.01, out hv_TrainingError, out hv_ErrorLog);
                //输入：字符区域、原图和分类器；输出 Classes 字符和 Confidence 置信度。
                hv_Classes.Dispose(); hv_Confidence.Dispose();
                HOperatorSet.DoOcrMultiClassMlp(ho_Characters, ho_GrayImage, hv_OCRHandle,
                    out hv_Classes, out hv_Confidence);
                //输入：Classes 字符数组；输出：拼接后的识别字符串。
                hv_RecognizedText.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RecognizedText = hv_Classes.TupleJoin(
                        "");
                }
                //输入：训练好的分类器；输出：OCR 模型文件。
                HOperatorSet.WriteOcrClassMlp(hv_OCRHandle, "C:/Users/16348/Desktop/课件/实战/12.omc");
                //输入：OCRHandle；释放分类器内存。
                HOperatorSet.ClearOcrClassMlp(hv_OCRHandle);
                //输入：识别结果；输出：Success；结果为 GLOSSED 时为 true。
                hv_Success.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Success = new HTuple(hv_RecognizedText.TupleEqual(
                        "GLOSSED"));
                }
                //显示原图。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }
                //设置区域显示为轮廓。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetDraw(HDevWindowStack.GetActive(), "margin");
                }
                //设置轮廓线宽。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetLineWidth(HDevWindowStack.GetActive(), 3);
                }
                //设置显示颜色为绿色。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetColor(HDevWindowStack.GetActive(), "green");
                }
                //显示分割出的字符区域。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Characters, HDevWindowStack.GetActive());
                }
                #region 图像自适应窗格大小
                //使图像自适应窗格大小。
                HOperatorSet.GetImageSize(ho_Image, out HTuple hv_ImageWidth, out HTuple hv_ImageHeight);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_ImageWidth, hv_ImageHeight);
                //显示原图。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }
                //显示分割出的字符区域。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Characters, HDevWindowStack.GetActive());
                }
                #endregion

                //输入：Characters；输出每个字符的中心坐标。
                hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
                HOperatorSet.AreaCenter(ho_Characters, out hv_Area, out hv_Row, out hv_Column);
                //在字符上方显示训练标签。
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispText(hv_WindowHandle, ((((((new HTuple("G")).TupleConcat("L")).TupleConcat(
                        "O")).TupleConcat("S")).TupleConcat("S")).TupleConcat("E")).TupleConcat(
                        "D"), "image", hv_Row + 110, hv_Column, "green", "box", "false");
                }
                //显示训练完成和验证结果。
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispText(hv_WindowHandle, (new HTuple("Training complete")).TupleConcat(
                        "Verification: " + hv_RecognizedText), "window", 12, 12, "green", "box",
                        "true");
                }
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();
                ho_GrayImage.Dispose();
                ho_BrightAll.Dispose();
                ho_BrightComponents.Dispose();
                ho_LargeBrightComponents.Dispose();
                ho_GroupedCandidates.Dispose();
                ho_GroupedUnion.Dispose();
                ho_ConnectedGroups.Dispose();
                ho_LargestTextGroup.Dispose();
                ho_TextROI.Dispose();
                ho_GrayROI.Dispose();
                ho_BrightRegion.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_CharacterCandidates.Dispose();
                ho_Characters.Dispose();

                hv_Row1.Dispose();
                hv_Col1.Dispose();
                hv_Row2.Dispose();
                hv_Col2.Dispose();
                hv_NumberCharacters.Dispose();
                hv_CharacterNames.Dispose();
                hv_CharacterCount.Dispose();
                hv_OCRHandle.Dispose();
                hv_TrainingError.Dispose();
                hv_ErrorLog.Dispose();
                hv_Classes.Dispose();
                hv_Confidence.Dispose();
                hv_RecognizedText.Dispose();
                hv_Success.Dispose();
                //hv_WindowHandle.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();

                throw HDevExpDefaultException;
            }
            ho_Image.Dispose();
            ho_GrayImage.Dispose();
            ho_BrightAll.Dispose();
            ho_BrightComponents.Dispose();
            ho_LargeBrightComponents.Dispose();
            ho_GroupedCandidates.Dispose();
            ho_GroupedUnion.Dispose();
            ho_ConnectedGroups.Dispose();
            ho_LargestTextGroup.Dispose();
            ho_TextROI.Dispose();
            ho_GrayROI.Dispose();
            ho_BrightRegion.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_CharacterCandidates.Dispose();
            ho_Characters.Dispose();

            hv_Row1.Dispose();
            hv_Col1.Dispose();
            hv_Row2.Dispose();
            hv_Col2.Dispose();
            hv_NumberCharacters.Dispose();
            hv_CharacterNames.Dispose();
            hv_CharacterCount.Dispose();
            hv_OCRHandle.Dispose();
            hv_TrainingError.Dispose();
            hv_ErrorLog.Dispose();
            hv_Classes.Dispose();
            hv_Confidence.Dispose();
            hv_RecognizedText.Dispose();
            hv_Success.Dispose();
            hv_WindowHandle.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
        }
        /// <summary>
        /// 读取图片并识别（实时取图）
        /// </summary>
        /// <param name="ho_Image">图片</param>
        /// <param name="hv_WindowHandle">句柄</param>
        /// <param name="upValue">二值化阈值上限</param>
        /// <param name="downValue">二值化阈值下限</param>
        public async Task<AppResultHelper<string>> ReadImageOcr(HObject ho_Image, HTuple hv_WindowHandle, int upValue, int downValue)
        {
            // Local iconic variables 

            HObject ho_GrayImage, ho_BrightAll;
            HObject ho_BrightComponents, ho_LargeBrightComponents, ho_GroupedCandidates;
            HObject ho_GroupedUnion, ho_ConnectedGroups, ho_LargestTextGroup;
            HObject ho_TextROI, ho_GrayROI, ho_BrightRegion, ho_ConnectedRegions;
            HObject ho_CharacterCandidates, ho_Characters;

            // Local control variables 

            HTuple hv_Row1 = new HTuple(), hv_Col1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Col2 = new HTuple();
            HTuple hv_NumberCharacters = new HTuple(), hv_CharacterNames = new HTuple();
            HTuple hv_CharacterCount = new HTuple(), hv_OCRHandle = new HTuple();
            HTuple hv_TrainingError = new HTuple(), hv_ErrorLog = new HTuple();
            HTuple hv_Classes = new HTuple(), hv_Confidence = new HTuple();
            HTuple hv_RecognizedText = new HTuple(), hv_Success = new HTuple();
            HTuple hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_BrightAll);
            HOperatorSet.GenEmptyObj(out ho_BrightComponents);
            HOperatorSet.GenEmptyObj(out ho_LargeBrightComponents);
            HOperatorSet.GenEmptyObj(out ho_GroupedCandidates);
            HOperatorSet.GenEmptyObj(out ho_GroupedUnion);
            HOperatorSet.GenEmptyObj(out ho_ConnectedGroups);
            HOperatorSet.GenEmptyObj(out ho_LargestTextGroup);
            HOperatorSet.GenEmptyObj(out ho_TextROI);
            HOperatorSet.GenEmptyObj(out ho_GrayROI);
            HOperatorSet.GenEmptyObj(out ho_BrightRegion);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_CharacterCandidates);
            HOperatorSet.GenEmptyObj(out ho_Characters);
            try
            {
                //GLOSSED OCR 训练程序：先运行本程序，再运行识别程序。
                //处理流程：读取图像 -> 阈值分割 -> 找最大文字区域 -> 分割字符 -> 训练 OCR。
                //图片、训练文件和模型路径均直接写在代码中。

                //输入：图片路径；输出：Image 图像对象。

                //输入：彩色 Image；输出：GrayImage 灰度图。
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                //输入：GrayImage；输出：BrightAll；150 到 255 保留亮字符。
                ho_BrightAll.Dispose();
                HOperatorSet.Threshold(ho_GrayImage, out ho_BrightAll, downValue, upValue);
                //输入：BrightAll；输出：BrightComponents；分离连通区域。
                ho_BrightComponents.Dispose();
                HOperatorSet.Connection(ho_BrightAll, out ho_BrightComponents);
                //输入：BrightComponents；输出：LargeBrightComponents；按面积、高度、宽度过滤小噪声。
                ho_LargeBrightComponents.Dispose();
                HOperatorSet.SelectShape(ho_BrightComponents, out ho_LargeBrightComponents,
                    ((new HTuple("area")).TupleConcat("height")).TupleConcat("width"), "and",
                    ((new HTuple(300)).TupleConcat(20)).TupleConcat(10), ((new HTuple(50000)).TupleConcat(
                    300)).TupleConcat(300));
                //输入：候选区域；输出：GroupedCandidates；横向 121、纵向 5，连接同一行字符。
                ho_GroupedCandidates.Dispose();
                HOperatorSet.DilationRectangle1(ho_LargeBrightComponents, out ho_GroupedCandidates,
                    121, 5);
                //输入：GroupedCandidates；输出：GroupedUnion；合并候选区域。
                ho_GroupedUnion.Dispose();
                HOperatorSet.Union1(ho_GroupedCandidates, out ho_GroupedUnion);
                //输入：GroupedUnion；输出：ConnectedGroups；得到合并区域。
                ho_ConnectedGroups.Dispose();
                HOperatorSet.Connection(ho_GroupedUnion, out ho_ConnectedGroups);
                //输入：ConnectedGroups；输出：LargestTextGroup；选择面积最大的区域。
                ho_LargestTextGroup.Dispose();
                HOperatorSet.SelectShapeStd(ho_ConnectedGroups, out ho_LargestTextGroup, "max_area",
                    70);
                //输入：最大区域；输出：Row1、Col1、Row2、Col2 边界坐标。
                hv_Row1.Dispose(); hv_Col1.Dispose(); hv_Row2.Dispose(); hv_Col2.Dispose();
                HOperatorSet.SmallestRectangle1(ho_LargestTextGroup, out hv_Row1, out hv_Col1,
                    out hv_Row2, out hv_Col2);
                //输入：四个边界坐标；输出：TextROI；生成动态文字 ROI。
                ho_TextROI.Dispose();
                HOperatorSet.GenRectangle1(out ho_TextROI, hv_Row1, hv_Col1, hv_Row2, hv_Col2);
                //输入：GrayImage 和 TextROI；输出：GrayROI；限制文字处理范围。
                ho_GrayROI.Dispose();
                HOperatorSet.ReduceDomain(ho_GrayImage, ho_TextROI, out ho_GrayROI);
                //输入：GrayROI；输出：BrightRegion；再次提取亮字符。
                ho_BrightRegion.Dispose();
                HOperatorSet.Threshold(ho_GrayROI, out ho_BrightRegion, downValue, upValue);
                //输入：BrightRegion；输出：ConnectedRegions；分离单个字符。
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_BrightRegion, out ho_ConnectedRegions);
                //输入：ConnectedRegions；输出：CharacterCandidates；过滤不符合尺寸的区域。
                ho_CharacterCandidates.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_CharacterCandidates, (
                    (new HTuple("area")).TupleConcat("height")).TupleConcat("width"), "and",
                    ((new HTuple(5000)).TupleConcat(120)).TupleConcat(40), ((new HTuple(50000)).TupleConcat(
                    260)).TupleConcat(250));
                //输入：字符候选区域；输出：Characters；按列从左到右排序。
                ho_Characters.Dispose();
                HOperatorSet.SortRegion(ho_CharacterCandidates, out ho_Characters, "character",
                    "true", "column");
                //输入：Characters；输出：NumberCharacters；统计字符数量。
                hv_NumberCharacters.Dispose();
                HOperatorSet.CountObj(ho_Characters, out hv_NumberCharacters);
                //字符数量不是 7 时停止。
                if ((int)(new HTuple(hv_NumberCharacters.TupleNotEqual(7))) != 0)
                {
                    //输出错误信息。
                    //throw new HalconException(new HTuple("Expected 7 characters, but segmented ") + hv_NumberCharacters);
                    //结束数量判断。
                    HOperatorSet.DispObj(ho_Characters, hv_WindowHandle);
                    return AppResultHelper<string>.Fail("字符数量错误");
                }
                //输入：字符区域、原图、字符标签；输出：训练文件。
                HOperatorSet.WriteOcrTrainf(ho_Characters, ho_GrayImage, ((((((new HTuple("G")).TupleConcat(
                    "L")).TupleConcat("O")).TupleConcat("S")).TupleConcat("S")).TupleConcat(
                    "E")).TupleConcat("D"), "C:/Users/16348/Desktop/课件/实战/12.trf");
                //输入：训练文件；输出：类别名称 CharacterNames 和样本数量 CharacterCount。
                hv_CharacterNames.Dispose(); hv_CharacterCount.Dispose();
                HOperatorSet.ReadOcrTrainfNames("C:/Users/16348/Desktop/课件/实战/12.trf",
                    out hv_CharacterNames, out hv_CharacterCount);
                //输入：类别名称和 MLP 参数；输出：OCRHandle 分类器句柄。
                hv_OCRHandle.Dispose();
                HOperatorSet.CreateOcrClassMlp(8, 10, "constant", "default", hv_CharacterNames,
                    20, "normalization", 10, 42, out hv_OCRHandle);
                //输入：分类器和训练文件；200 是最大训练次数；输出训练误差和日志。
                hv_TrainingError.Dispose(); hv_ErrorLog.Dispose();
                HOperatorSet.TrainfOcrClassMlp(hv_OCRHandle, "C:/Users/16348/Desktop/课件/实战/12.trf",
                    200, 0.01, 0.01, out hv_TrainingError, out hv_ErrorLog);
                //输入：字符区域、原图和分类器；输出 Classes 字符和 Confidence 置信度。
                hv_Classes.Dispose(); hv_Confidence.Dispose();
                HOperatorSet.DoOcrMultiClassMlp(ho_Characters, ho_GrayImage, hv_OCRHandle,
                    out hv_Classes, out hv_Confidence);
                //输入：Classes 字符数组；输出：拼接后的识别字符串。
                hv_RecognizedText.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RecognizedText = hv_Classes.TupleJoin("");
                }
                //输入：训练好的分类器；输出：OCR 模型文件。
                HOperatorSet.WriteOcrClassMlp(hv_OCRHandle, "C:/Users/16348/Desktop/课件/实战/12.omc");
                //输入：OCRHandle；释放分类器内存。
                HOperatorSet.ClearOcrClassMlp(hv_OCRHandle);
                //输入：识别结果；输出：Success；结果为 GLOSSED 时为 true。
                hv_Success.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Success = new HTuple(hv_RecognizedText.TupleEqual(
                        "GLOSSED"));
                }


                //显示原图。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }
                //设置区域显示为轮廓。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetDraw(HDevWindowStack.GetActive(), "margin");
                }
                //设置轮廓线宽。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetLineWidth(HDevWindowStack.GetActive(), 3);
                }
                //设置显示颜色为绿色。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.SetColor(HDevWindowStack.GetActive(), "green");
                }
                //显示分割出的字符区域。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Characters, HDevWindowStack.GetActive());
                }
                #region 图像自适应窗格大小
                //使图像自适应窗格大小。
                HOperatorSet.GetImageSize(ho_Image, out HTuple hv_ImageWidth, out HTuple hv_ImageHeight);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_ImageWidth, hv_ImageHeight);
                //显示原图。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }
                //显示分割出的字符区域。
                if (HDevWindowStack.IsOpen())
                {
                    HOperatorSet.DispObj(ho_Characters, HDevWindowStack.GetActive());
                }
                #endregion

                //输入：Characters；输出每个字符的中心坐标。
                hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
                HOperatorSet.AreaCenter(ho_Characters, out hv_Area, out hv_Row, out hv_Column);
                //在字符上方显示训练标签。
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispText(hv_WindowHandle, ((((((new HTuple("G")).TupleConcat("L")).TupleConcat(
                        "O")).TupleConcat("S")).TupleConcat("S")).TupleConcat("E")).TupleConcat(
                        "D"), "image", hv_Row + 110, hv_Column, "green", "box", "false");
                }
                //显示训练完成和验证结果。
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispText(hv_WindowHandle, (new HTuple("Training complete")).TupleConcat(
                        "Verification: " + hv_RecognizedText), "window", 12, 12, "green", "box",
                        "true");
                }
                return AppResultHelper<string>.Success(hv_RecognizedText);
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Image.Dispose();
                ho_GrayImage.Dispose();
                ho_BrightAll.Dispose();
                ho_BrightComponents.Dispose();
                ho_LargeBrightComponents.Dispose();
                ho_GroupedCandidates.Dispose();
                ho_GroupedUnion.Dispose();
                ho_ConnectedGroups.Dispose();
                ho_LargestTextGroup.Dispose();
                ho_TextROI.Dispose();
                ho_GrayROI.Dispose();
                ho_BrightRegion.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_CharacterCandidates.Dispose();
                ho_Characters.Dispose();

                hv_Row1.Dispose();
                hv_Col1.Dispose();
                hv_Row2.Dispose();
                hv_Col2.Dispose();
                hv_NumberCharacters.Dispose();
                hv_CharacterNames.Dispose();
                hv_CharacterCount.Dispose();
                hv_OCRHandle.Dispose();
                hv_TrainingError.Dispose();
                hv_ErrorLog.Dispose();
                hv_Classes.Dispose();
                hv_Confidence.Dispose();
                hv_RecognizedText.Dispose();
                hv_Success.Dispose();
                //hv_WindowHandle.Dispose();
                hv_Area.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();

                throw HDevExpDefaultException;
            }
            ho_Image.Dispose();
            ho_GrayImage.Dispose();
            ho_BrightAll.Dispose();
            ho_BrightComponents.Dispose();
            ho_LargeBrightComponents.Dispose();
            ho_GroupedCandidates.Dispose();
            ho_GroupedUnion.Dispose();
            ho_ConnectedGroups.Dispose();
            ho_LargestTextGroup.Dispose();
            ho_TextROI.Dispose();
            ho_GrayROI.Dispose();
            ho_BrightRegion.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_CharacterCandidates.Dispose();
            ho_Characters.Dispose();

            hv_Row1.Dispose();
            hv_Col1.Dispose();
            hv_Row2.Dispose();
            hv_Col2.Dispose();
            hv_NumberCharacters.Dispose();
            hv_CharacterNames.Dispose();
            hv_CharacterCount.Dispose();
            hv_OCRHandle.Dispose();
            hv_TrainingError.Dispose();
            hv_ErrorLog.Dispose();
            hv_Classes.Dispose();
            hv_Confidence.Dispose();
            hv_RecognizedText.Dispose();
            hv_Success.Dispose();
            hv_WindowHandle.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
        }

        // Procedures 
        // External procedures 
        // Chapter: Develop
        // Short Description: Open a new graphics window that preserves the aspect ratio of the given image. 
        public void dev_open_window_fit_image(HObject ho_Image, HTuple hv_Row, HTuple hv_Column,
            HTuple hv_WidthLimit, HTuple hv_HeightLimit, out HTuple hv_WindowHandle)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_MinWidth = new HTuple(), hv_MaxWidth = new HTuple();
            HTuple hv_MinHeight = new HTuple(), hv_MaxHeight = new HTuple();
            HTuple hv_ResizeFactor = new HTuple(), hv_ImageWidth = new HTuple();
            HTuple hv_ImageHeight = new HTuple(), hv_TempWidth = new HTuple();
            HTuple hv_TempHeight = new HTuple(), hv_WindowWidth = new HTuple();
            HTuple hv_WindowHeight = new HTuple();
            // Initialize local and output iconic variables 
            hv_WindowHandle = new HTuple();
            try
            {
                //This procedure opens a new graphics window and adjusts the size
                //such that it fits into the limits specified by WidthLimit
                //and HeightLimit, but also maintains the correct image aspect ratio.
                //
                //If it is impossible to match the minimum and maximum extent requirements
                //at the same time (f.e. if the image is very long but narrow),
                //the maximum value gets a higher priority,
                //
                //Parse input tuple WidthLimit
                if ((int)((new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(
                    0))).TupleOr(new HTuple(hv_WidthLimit.TupleLess(0)))) != 0)
                {
                    hv_MinWidth.Dispose();
                    hv_MinWidth = 500;
                    hv_MaxWidth.Dispose();
                    hv_MaxWidth = 800;
                }
                else if ((int)(new HTuple((new HTuple(hv_WidthLimit.TupleLength())).TupleEqual(
                    1))) != 0)
                {
                    hv_MinWidth.Dispose();
                    hv_MinWidth = 0;
                    hv_MaxWidth.Dispose();
                    hv_MaxWidth = new HTuple(hv_WidthLimit);
                }
                else
                {
                    hv_MinWidth.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MinWidth = hv_WidthLimit.TupleSelect(
                            0);
                    }
                    hv_MaxWidth.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaxWidth = hv_WidthLimit.TupleSelect(
                            1);
                    }
                }
                //Parse input tuple HeightLimit
                if ((int)((new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(
                    0))).TupleOr(new HTuple(hv_HeightLimit.TupleLess(0)))) != 0)
                {
                    hv_MinHeight.Dispose();
                    hv_MinHeight = 400;
                    hv_MaxHeight.Dispose();
                    hv_MaxHeight = 600;
                }
                else if ((int)(new HTuple((new HTuple(hv_HeightLimit.TupleLength())).TupleEqual(
                    1))) != 0)
                {
                    hv_MinHeight.Dispose();
                    hv_MinHeight = 0;
                    hv_MaxHeight.Dispose();
                    hv_MaxHeight = new HTuple(hv_HeightLimit);
                }
                else
                {
                    hv_MinHeight.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MinHeight = hv_HeightLimit.TupleSelect(
                            0);
                    }
                    hv_MaxHeight.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_MaxHeight = hv_HeightLimit.TupleSelect(
                            1);
                    }
                }
                //
                //Test, if window size has to be changed.
                hv_ResizeFactor.Dispose();
                hv_ResizeFactor = 1;
                hv_ImageWidth.Dispose(); hv_ImageHeight.Dispose();
                HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
                //First, expand window to the minimum extents (if necessary).
                if ((int)((new HTuple(hv_MinWidth.TupleGreater(hv_ImageWidth))).TupleOr(new HTuple(hv_MinHeight.TupleGreater(
                    hv_ImageHeight)))) != 0)
                {
                    hv_ResizeFactor.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_ResizeFactor = (((((hv_MinWidth.TupleReal()
                            ) / hv_ImageWidth)).TupleConcat((hv_MinHeight.TupleReal()) / hv_ImageHeight))).TupleMax()
                            ;
                    }
                }
                hv_TempWidth.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_TempWidth = hv_ImageWidth * hv_ResizeFactor;
                }
                hv_TempHeight.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_TempHeight = hv_ImageHeight * hv_ResizeFactor;
                }
                //Then, shrink window to maximum extents (if necessary).
                if ((int)((new HTuple(hv_MaxWidth.TupleLess(hv_TempWidth))).TupleOr(new HTuple(hv_MaxHeight.TupleLess(
                    hv_TempHeight)))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_ResizeFactor = hv_ResizeFactor * ((((((hv_MaxWidth.TupleReal()
                                ) / hv_TempWidth)).TupleConcat((hv_MaxHeight.TupleReal()) / hv_TempHeight))).TupleMin()
                                );
                            hv_ResizeFactor.Dispose();
                            hv_ResizeFactor = ExpTmpLocalVar_ResizeFactor;
                        }
                    }
                }
                hv_WindowWidth.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_WindowWidth = hv_ImageWidth * hv_ResizeFactor;
                }
                hv_WindowHeight.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_WindowHeight = hv_ImageHeight * hv_ResizeFactor;
                }
                //Resize window
                HOperatorSet.SetWindowAttr("background_color", "black");
                HOperatorSet.OpenWindow(hv_Row, hv_Column, hv_WindowWidth, hv_WindowHeight, 0, "visible", "", out hv_WindowHandle);
                HDevWindowStack.Push(hv_WindowHandle);
                if (HDevWindowStack.IsOpen())
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HOperatorSet.SetPart(HDevWindowStack.GetActive(), 0, 0, hv_ImageHeight - 1,
                            hv_ImageWidth - 1);
                    }
                }

                hv_MinWidth.Dispose();
                hv_MaxWidth.Dispose();
                hv_MinHeight.Dispose();
                hv_MaxHeight.Dispose();
                hv_ResizeFactor.Dispose();
                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_TempWidth.Dispose();
                hv_TempHeight.Dispose();
                hv_WindowWidth.Dispose();
                hv_WindowHeight.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_MinWidth.Dispose();
                hv_MaxWidth.Dispose();
                hv_MinHeight.Dispose();
                hv_MaxHeight.Dispose();
                hv_ResizeFactor.Dispose();
                hv_ImageWidth.Dispose();
                hv_ImageHeight.Dispose();
                hv_TempWidth.Dispose();
                hv_TempHeight.Dispose();
                hv_WindowWidth.Dispose();
                hv_WindowHeight.Dispose();

                throw HDevExpDefaultException;
            }
        }
    }

}
