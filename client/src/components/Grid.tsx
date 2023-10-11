export type gridProps = {
  children: any;
  id?: string;
  className?: string;
};

export default function Grid({ children, id, className }: gridProps) {
  return (
    <div id={id} className={`grid grid-cols-4 grid-rows-4 gap-4 ${className}`}>
      {children}
    </div>
  );
}
