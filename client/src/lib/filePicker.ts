export default function openFilePicker() {
  return new Promise<File[]>((resolve, reject) => {
    const fileInput = document.createElement("input");

    fileInput.setAttribute("type", "file");
    fileInput.setAttribute("multiple", "");
    fileInput.addEventListener("change", (ev) => {
      const fileList: FileList = (ev as any).target.files;
      resolve(Array.from(fileList));
    });

    fileInput.click();
  });
}
