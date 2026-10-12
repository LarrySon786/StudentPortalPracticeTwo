window.downloadPdf = (fileName, base64Data) => {
    const link = document.createElement("a");
    link.href = `data:application/pdf;base64,${base64Data}`;
    link.download = fileName;

    document.body.appendChild(link);
    link.click();
    link.remove();
}